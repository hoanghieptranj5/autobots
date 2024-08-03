using IsolatedWorkerAutobot.Constants;

namespace IsolatedWorkerAutobot.Functions;

public class ElectricCalculatorFunctions(IElectricPriceService electricPriceService, ICalculationLogic calculationLogic)
{
    [Authorize]
    [OpenApiOperation("GetElectricPrices", "ElectricPrice", Summary = "GetElectricPrices",
        Description = "This gets a list of electricPrice.", Visibility = OpenApiVisibilityType.Important)]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = AuthCode.Token, In = OpenApiSecurityLocationType.Query)]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "text/plain", typeof(string),
        Summary = "The response", Description = "This returns the response")]
    [Function("GetElectricPrices")]
    public async Task<IActionResult> GetElectricPrices(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "electricPrices")]
        HttpRequestData req,
        ILogger log)
    {
        var results = await electricPriceService.GetAll();
        return new OkObjectResult(results);
    }

    [Authorize]
    [OpenApiOperation("CalculateUsage", "Usage", Summary = "CalculateUsage",
        Description = "This gets a list of CalculateUsage.", Visibility = OpenApiVisibilityType.Important)]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = AuthCode.Token, In = OpenApiSecurityLocationType.Query)]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "text/plain", typeof(string),
        Summary = "The response", Description = "This returns the response")]
    [OpenApiParameter("value", Type = typeof(int))]
    [Function("CalculateUsage")]
    public async Task<IActionResult> CalculateUsage(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "electricPrices/usage/{value}")]
        HttpRequestData req,
        int value,
        ILogger log)
    {
        var result = await calculationLogic.CalculateAsync(value);
        return new OkObjectResult(result);
    }
}
