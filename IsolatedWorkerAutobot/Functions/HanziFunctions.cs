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
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = AuthCode.Token, In = OpenApiSecurityLocationType.Header)]
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
        var result = await _hanziService.GetSingle(id);
        return new OkObjectResult(result);
    }

    [Authorize]
    [OpenApiOperation("GetRandomList", "Hanzi", Summary = "GetRandomList",
        Description = "This gets a random Hanzi list, length is passed by user.", Visibility = OpenApiVisibilityType.Important)]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = AuthCode.Token, In = OpenApiSecurityLocationType.Header)]
    [OpenApiParameter("length", Type = typeof(int))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "text/plain", typeof(string),
        Summary = "The response", Description = "This returns the response")]
    [Function("GetRandomList")]
    public async Task<IActionResult> GetRandomList(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "hanzi/random/{length}")]
        HttpRequestData req,
        int length,
        ILogger log)
    {
        try
        {
            var result = await _hanziService.GetRandomList(length);
            return new OkObjectResult(result);
        }
        catch (Exception ex)
        {
            log.LogError(ex.Message);
            return new StatusCodeResult((int)HttpStatusCode.BadRequest);
        }
    }
}
