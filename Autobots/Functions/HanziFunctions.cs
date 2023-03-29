using System.Threading.Tasks;
using HanziCollector.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Autobots.Functions;

public class HanziFunctions
{
    private readonly ITextDocumentReader _textDocumentReader;

    public HanziFunctions(ITextDocumentReader textDocumentReader)
    {
        _textDocumentReader = textDocumentReader;
    }

    [ApiExplorerSettings(GroupName = "Hanzi")]
    [FunctionName("GetHanzi")]
    public async Task<IActionResult> GetHanzi(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "hanzi/{filePath}")] HttpRequest req,
        string filePath,
        ILogger log)
    {
        var results = _textDocumentReader.ReadToCharArray(filePath);
        return new OkObjectResult(results);
    }
}