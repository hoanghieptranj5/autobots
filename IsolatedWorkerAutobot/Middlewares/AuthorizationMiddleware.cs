using System.Net;
using System.Security.Claims;
using IAM.Data.Abstraction;
using IAM.Helper;
using IsolatedWorkerAutobot.CustomAttributes;
using IsolatedWorkerAutobot.Middlewares.Helpers;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace IsolatedWorkerAutobot.Middlewares;

public class AuthorizationMiddleware : IFunctionsWorkerMiddleware
{
  private readonly ILogger<AuthorizationMiddleware> _logger;
  private readonly IUserService _userService;

  public AuthorizationMiddleware(ILogger<AuthorizationMiddleware> logger, IUserService userService)
  {
    _logger = logger;
    _userService = userService;
  }

  public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
  {
    var targetMethod = TargetMethodHelper.GetTargetFunctionMethod(context);
    var authAttrs = TargetMethodHelper.GetFunctionMethodAttribute<AuthorizeAttribute>(targetMethod);
    var allowAllAttrs = TargetMethodHelper.GetFunctionMethodAttribute<AllowAnonymousAttribute>(targetMethod);

    if (authAttrs.Any())
    {
      _logger.LogDebug($"This method {targetMethod.Name} requires authorization.");
      
      const string key = "code";
      var requestData = await context.GetHttpRequestDataAsync();
      var token = requestData.Query.Get(key);
      if (token == null)
      {
        var req = await context.GetHttpRequestDataAsync();
        var res = req!.CreateResponse();
        res.StatusCode = HttpStatusCode.Unauthorized;
        await res.WriteAsJsonAsync("Access Denied\n\nYou have attempted to access a protected resource that requires authentication using a JSON Web Token (JWT). To access this endpoint, you must include a valid JWT token in the request headers.\n\nPlease ensure that you have obtained an authentication token from our authentication service. You can do this by following our authentication process, which typically involves logging in and receiving a token in response.\n\nOnce you have obtained a valid JWT token, make sure to include it in your request headers as follows:\n\nAuthorization: Bearer {your_jwt_token_here}\n\nIf you believe you have received this message in error or need assistance with authentication, please contact our support team at support@example.com for further assistance.\n\nThank you for using our services.");
        context.GetInvocationResult().Value = res;
        return;
      }

      var isValid = JwtHelper.ValidateToken(token);
      if (!isValid)
      {
        var req = await context.GetHttpRequestDataAsync();
        var res = req!.CreateResponse();
        res.StatusCode = HttpStatusCode.Unauthorized;
        await res.WriteAsJsonAsync("Invalid or Expired Token\n\nThe JSON Web Token (JWT) provided in the request is either invalid or has expired. To access this endpoint, you must provide a valid and unexpired JWT token.\n\nPossible reasons for this error include:\n- The token has expired: JWT tokens have a limited lifespan for security purposes. Please obtain a fresh token by re-authenticating.\n- The token signature is invalid: Ensure that the provided token is signed correctly and has not been tampered with.\n- The token format is incorrect: Verify that you have included the token in the request headers using the 'Authorization' header in the format 'Bearer {your_jwt_token_here}'.\n\nIf you believe you have received this message in error or need assistance with obtaining a valid token, please contact our support team at support@example.com for further assistance.\n\nThank you for using our services.");
        context.GetInvocationResult().Value = res;
        return;
      }
      
      var expiration = JwtHelper.GetClaim(token, ClaimTypes.Expiration);
      _logger.LogDebug(expiration);
      DateTime.TryParse(expiration, out DateTime expirationDateTime);
      _logger.LogDebug(expirationDateTime.ToShortDateString());
      if (expirationDateTime < DateTime.Now)
      {
        var req = await context.GetHttpRequestDataAsync();
        var res = req!.CreateResponse();
        res.StatusCode = HttpStatusCode.Unauthorized;
        await res.WriteAsJsonAsync("token expired.");
        context.GetInvocationResult().Value = res;
        return;
      }
    }
    else if (allowAllAttrs.Any()) _logger.LogDebug($"This method {targetMethod.Name} allow all inbounds.");

    await next(context);
  }
}