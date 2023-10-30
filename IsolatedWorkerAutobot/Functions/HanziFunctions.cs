using HanziCollector.Abstraction;
using IsolatedWorkerAutobot.Constants;

namespace IsolatedWorkerAutobot.Functions;

public class HanziFunctions
{
    private readonly IHanziService _hanziService;

    public HanziFunctions(IHanziService hanziService)
    {
        _hanziService = hanziService;
    }

    [Authorize]
    [OpenApiOperation("GetInformationSingle", "Hanzi", Summary = "GetInformationSingle",
        Description = "This gets information of single hanzi.", Visibility = OpenApiVisibilityType.Important)]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = AuthCode.Token, In = OpenApiSecurityLocationType.Query)]
    [OpenApiParameter("id", Type = typeof(string))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "text/plain", typeof(string),
        Summary = "The response", Description = "This returns the response")]
    [Function("GetInformationSingle")]
    public async Task<IActionResult> GetInformationSingle(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "hanzi/{id}")]
        HttpRequestData req,
        string id,
        ILogger log)
    {
        var result = await _hanziService.GetHanziInformationFromHvDic(id);
        return new OkObjectResult(result);
    }
}
