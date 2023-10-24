namespace IsolatedWorkerAutobot.Functions;

public class ElectricCalculatorFunctions
{
  private readonly IElectricPriceService _electricPriceService;

  public ElectricCalculatorFunctions(IElectricPriceService electricPriceService)
  {
    _electricPriceService = electricPriceService;
  }

  [Authorize]
  [OpenApiOperation("GetElectricPrices", "ElectricPrice", Summary = "GetElectricPrices",
    Description = "This gets a list of electricPrice.", Visibility = OpenApiVisibilityType.Important)]
  [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
  [OpenApiResponseWithBody(HttpStatusCode.OK, "text/plain", typeof(string),
    Summary = "The response", Description = "This returns the response")]
  [Function("GetElectricPrices")]
  public async Task<IActionResult> GetElectricPrices(
    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "electricPrices")]
    HttpRequestData req,
    ILogger log)
  {
    var results = await _electricPriceService.GetAll();
    return new OkObjectResult(results);
  }
}