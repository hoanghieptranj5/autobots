namespace IsolatedWorkerAutobot.Functions;

/// <summary>
///     From V4 version, we move from HttpRequest to use HttpRequestData
/// </summary>
public class HttpExampleFunction
{
    private readonly ILogger _logger;

    public HttpExampleFunction(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<HttpExampleFunction>();
    }

    [AllowAnonymous]
    // 👇👇👇👇👇 Add OpenAPI related decorators below 👇👇👇👇👇
    [OpenApiOperation("greeting", "greeting", Summary = "Greetings",
        Description = "This shows a welcome message.", Visibility = OpenApiVisibilityType.Important)]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "text/plain", typeof(string),
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
