using System.Security.Claims;
using IAM.Data.Abstraction;
using IAM.Helper;
using IsolatedWorkerAutobot.CustomAttributes;
using IsolatedWorkerAutobot.Middlewares.Helpers;
using Microsoft.Azure.Functions.Worker;
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
    var authAttrs = TargetMethodHelper.GetFunctionMethodAttribute<AuthorizedAttribute>(targetMethod);
    var allowAllAttrs = TargetMethodHelper.GetFunctionMethodAttribute<AllowAllAttribute>(targetMethod);

    if (authAttrs.Any())
    {
      _logger.LogDebug($"This method {targetMethod.Name} requires authorization.");
      
      const string key = "code";
      var requestData = await context.GetHttpRequestDataAsync();
      var token = requestData.Query.Get(key);
      if (token == null)
      {
        throw new Exception("Token is not found.");
      }

      var isValid = JwtHelper.ValidateToken(token);
      if (!isValid)
      {
        throw new Exception("Token is expired or invalid.");
      }
      
      var email = JwtHelper.GetClaim(token, "email");
      _logger.LogTrace(email);
    }
    else if (allowAllAttrs.Any()) _logger.LogDebug($"This method {targetMethod.Name} allow all inbounds.");

    await next(context);
  }
}