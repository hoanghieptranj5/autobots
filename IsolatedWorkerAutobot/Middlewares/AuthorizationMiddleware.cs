using System.Net;
using IsolatedWorkerAutobot.CustomAttributes;
using IsolatedWorkerAutobot.Exceptions;
using IsolatedWorkerAutobot.Middlewares.Helpers;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace IsolatedWorkerAutobot.Middlewares;

public class AuthorizationMiddleware : IFunctionsWorkerMiddleware
{
  private readonly ILogger<AuthorizationMiddleware> _logger;

  public AuthorizationMiddleware(ILogger<AuthorizationMiddleware> logger)
  {
    _logger = logger;
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
      try
      {
        JwtTokenValidator.Verify(token);
      }
      catch (TokenUnavailableException ex)
      {
        const string message =
          "Access to this endpoint is unauthorized due to an invalid or expired JSON Web Token (JWT).";
        await MiddlewareResponseWriter.WriteAsJsonAsync(context, message, HttpStatusCode.Unauthorized);
        return;
      }
      catch (TokenExpiredException ex)
      {
        const string message = "Token has been expired. Please try to login again or use the refresh token instead.";
        await MiddlewareResponseWriter.WriteAsJsonAsync(context, message, HttpStatusCode.Unauthorized);
      }
      catch (Exception ex)
      {
        var message = $"Unexpected error when validating token: {ex.Message}";
        await MiddlewareResponseWriter.WriteAsJsonAsync(context, message, HttpStatusCode.InternalServerError);
      }
    }
    else if (allowAllAttrs.Any())
    {
      _logger.LogDebug($"This method {targetMethod.Name} allow all inbounds.");
    }

    await next(context);
  }
}