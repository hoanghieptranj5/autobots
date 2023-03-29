using System.Linq;
using System.Threading.Tasks;
using HanziCollector.Abstraction;
using HanziCollector.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Autobots.Functions;

[ApiExplorerSettings(GroupName = "汉字")]
public class HanziFunctions
{
    private readonly IHanziService _hanziService;

    public HanziFunctions(IHanziService hanziService)
    {
        _hanziService = hanziService;
    }

    [FunctionName("ImportByTxtFile")]
    public async Task<IActionResult> ImportByTxtFile(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "hanzi/{filePath}")] HttpRequest req,
        string filePath,
        ILogger log)
    {
        _hanziService.ImportFromTextDocumentFile(filePath);
        return new OkObjectResult("200 OK");
    }
}