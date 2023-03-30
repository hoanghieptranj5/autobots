using System;
using System.Linq;
using System.Threading.Tasks;
using HanziCollector.Abstraction;
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
        await _hanziService.ImportFromTextDocumentFile(filePath);
        return new OkObjectResult("200 OK");
    }
    
    [FunctionName("GetInformationOfHanzi")]
    public async Task<IActionResult> GetInformationOfHanzi(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "hanzi/{id}/hvdic")] HttpRequest req,
        string id,
        ILogger log)
    {
        var result = await _hanziService.GetHanziInformationFromHvDic(id);
        return new OkObjectResult(result);
    }
    
    [FunctionName("GetInformationOfMultipleHanzis")]
    public async Task<IActionResult> GetInformationOfMultipleHanzis(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "hanzi/多/{listId}/hvdic")] HttpRequest req,
        string listId,
        ILogger log)
    {
        var chars = listId.ToCharArray().Select(x => x.ToString());

        var result = chars.Select(async c => await _hanziService.GetHanziInformationFromHvDic(c))
            .Select(t => t.Result)
            .Where(i => i != null)
            .ToList();
        
        return new OkObjectResult(result);
    }
}