using AutoMapper;
using CosmosRepository.Entities.Vocabulary;
using HanziCollector.Abstraction;
using IAM.ValuedObjects;
using IsolatedWorkerAutobot.Constants;
using IsolatedWorkerAutobot.ValuedObjects;
using Newtonsoft.Json;

namespace IsolatedWorkerAutobot.Functions;

public class VocabularyFunctions
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IVocabularyDbService _vocabularyDbService;

    public VocabularyFunctions(ILoggerFactory loggerFactory, IVocabularyDbService userService, IMapper mapper)
    {
        _logger = loggerFactory.CreateLogger<VocabularyFunctions>();
        _vocabularyDbService = userService;
        _mapper = mapper;
    }

    [Authorize]
    [OpenApiOperation("GetVocabularyList", "Vocabulary", Summary = "GetVocabularyList",
        Description = "This gets a list of vocabularies.", Visibility = OpenApiVisibilityType.Important)]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = AuthCode.Token, In = OpenApiSecurityLocationType.Header)]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "text/plain", typeof(string),
        Summary = "The response", Description = "This returns the response")]
    [Function("GetVocabularyList")]
    public async Task<IActionResult> GetVocabularyList(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "vocabulary")]
        HttpRequestData req)
    {
        _logger.LogInformation("HTTP trigger function get list of users.");
        try
        {
            var result = await _vocabularyDbService.ReadAll();

            return new OkObjectResult(result);
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(ex.Message);
        }
    }

    [Authorize]
    [OpenApiOperation("AddSingleVocabulary", "Vocabulary", Summary = "AddSingleVocabulary",
        Description = "This adds a new vocabulary to system.", Visibility = OpenApiVisibilityType.Important)]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = AuthCode.Token, In = OpenApiSecurityLocationType.Header)]
    [OpenApiRequestBody("application/json", typeof(CreateVocabularyRequest), Description = "Parameters",
        Example = typeof(Vocabulary))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "text/plain", typeof(string),
        Summary = "The response", Description = "This returns the response")]
    [Function("AddSingleVocabulary")]
    public async Task<IActionResult> AddSingleVocabulary(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "vocabulary")]
        HttpRequestData req)
    {
        _logger.LogInformation("HTTP trigger function create new user");
        var msg = await req.ReadAsStringAsync();
        var createVocabularyRequest = JsonConvert.DeserializeObject<CreateVocabularyRequest>(msg);
        var mapped = _mapper.Map<Vocabulary>(createVocabularyRequest);

        var result = await _vocabularyDbService.SaveSingle(mapped);
        return new OkObjectResult(result);
    }
}
