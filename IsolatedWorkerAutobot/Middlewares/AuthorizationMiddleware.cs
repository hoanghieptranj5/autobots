using IsolatedWorkerAutobot.CustomAttributes;
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
    var targetMethod = MiddlewareHelper.GetTargetFunctionMethod(context);
    var authAttrs = MiddlewareHelper.GetFunctionMethodAttribute<AuthorizedAttribute>(targetMethod);
    var allowAllAttrs = MiddlewareHelper.GetFunctionMethodAttribute<AllowAllAttribute>(targetMethod);

    if (authAttrs.Any())
      _logger.LogDebug($"This method {targetMethod.Name} requires authorization.");
    else if (allowAllAttrs.Any()) _logger.LogDebug($"This method {targetMethod.Name} allow all inbounds.");

    await next(context);
  }
}