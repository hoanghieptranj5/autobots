using System.Linq;
using System.Threading.Tasks;
using HanziCollector.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Repositories.Models.HanziCollector;

namespace Autobots.Functions;

public class HanziFunctions
{
    private readonly ITextDocumentReader _textDocumentReader;
    private readonly IHanziDbService _hanziDbService;

    public HanziFunctions(ITextDocumentReader textDocumentReader, IHanziDbService hanziDbService)
    {
        _textDocumentReader = textDocumentReader;
        _hanziDbService = hanziDbService;
    }

    [ApiExplorerSettings(GroupName = "Hanzi")]
    [FunctionName("GetHanzi")]
    public async Task<IActionResult> GetHanzi(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "hanzi/{filePath}")] HttpRequest req,
        string filePath,
        ILogger log)
    {
        var results = _textDocumentReader.ReadToCharArray(filePath);
        await _hanziDbService.SaveSingle(new Hanzi() { Id = results.First() });
        return new OkObjectResult(results);
    }
    
    [ApiExplorerSettings(GroupName = "Hanzi")]
    [FunctionName("GetAll")]
    public async Task<IActionResult> GetAll(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "hanzi")] HttpRequest req,
        ILogger log)
    {
        var results = await _hanziDbService.ReadAll();
        return new OkObjectResult(results);
    }
    
    [ApiExplorerSettings(GroupName = "Hanzi")]
    [FunctionName("DeleteSingle")]
    public async Task<IActionResult> DeleteSingle(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "hanzi/{id}")] HttpRequest req,
        string id,
        ILogger log)
    {
        var results = await _hanziDbService.DeleteSingle(id);
        return new OkObjectResult(results);
    }
}