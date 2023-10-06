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
        const string message = "Access to this endpoint is unauthorized. Please provide a valid JSON Web Token (JWT) " +
                               "in the 'Authorization header of your request.";
        await MiddlewareResponseWriter.WriteAsJsonAsync(context, message, HttpStatusCode.Unauthorized);
        return;
      }

      var isValid = JwtHelper.ValidateToken(token);
      if (!isValid)
      {
        const string message = "Access to this endpoint is unauthorized due to an invalid or expired JSON Web Token (JWT).";
        await MiddlewareResponseWriter.WriteAsJsonAsync(context, message, HttpStatusCode.Unauthorized);
        return;
      }
      
      var expiration = JwtHelper.GetClaim(token, ClaimTypes.Expiration);
      _logger.LogDebug(expiration);
      DateTime.TryParse(expiration, out var expirationDateTime);
      _logger.LogDebug(expirationDateTime.ToShortDateString());
      if (expirationDateTime < DateTime.Now)
      {
        const string message = "Token has been expired. Please try to login again or use the refresh token instead.";
        await MiddlewareResponseWriter.WriteAsJsonAsync(context, message, HttpStatusCode.Unauthorized);
      }
    }
    else if (allowAllAttrs.Any())
      _logger.LogDebug($"This method {targetMethod.Name} allow all inbounds.");

    await next(context);
  }
}