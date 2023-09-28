using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace IsolatedWorkerAutobot.Functions
{
  /// <summary>
  /// From V4 version, we move from HttpRequest to use HttpRequestData
  /// </summary>
  public class HttpExample
  {
    private readonly ILogger _logger;

    public HttpExample(ILoggerFactory loggerFactory)
    {
      _logger = loggerFactory.CreateLogger<HttpExample>();
    }

    // 👇👇👇👇👇 Add OpenAPI related decorators below 👇👇👇👇👇
    [OpenApiOperation(operationId: "greeting", tags: new[] { "greeting" }, Summary = "Greetings",
      Description = "This shows a welcome message.", Visibility = OpenApiVisibilityType.Important)]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string),
      Summary = "The response", Description = "This returns the response")]
    // 👆👆👆👆👆 Add OpenAPI related decorators above 👆👆👆👆👆
    [Function("HttpExample")]
    public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
    {
      _logger.LogInformation("C# HTTP trigger function processed a request.");

      var response = req.CreateResponse(HttpStatusCode.OK);
      response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

      response.WriteString("Welcome to Azure Functions!");

      return response;
    }
  }
}