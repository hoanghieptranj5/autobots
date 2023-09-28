using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Azure.WebJobs.Host;
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
    var endpoint = context.Features.Get<IEndpointFeature>();
    if (endpoint != null)
    {
      _logger.LogInformation($"{endpoint.Endpoint.DisplayName}");
    }
    await next(context);
  }
}