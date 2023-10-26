using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using HanziCollector.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Repositories.Models.HanziCollector;

namespace Autobots.Functions;

[ApiExplorerSettings(GroupName = "Hanzi")]
public class HanziFunctions
{
    private readonly IHanziService _hanziService;

    public HanziFunctions(IHanziService hanziService)
    {
        _hanziService = hanziService;
    }

    [FunctionName("ImportByTxtFile")]
    [QueryStringParameter("takeFrom", "Number of chars needs translating", DataType = typeof(int), Required = false)]
    [QueryStringParameter("takeTo", "Number of chars needs translating", DataType = typeof(int), Required = false)]
    public async Task<IActionResult> ImportByTxtFile(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "hanzi/{filePath}")]
        HttpRequest req,
        string filePath,
        ILogger log)
    {
        var takeFrom = int.Parse(req.Query["takeFrom"]);
        var takeTo = int.Parse(req.Query["takeTo"]);
        var result = await _hanziService.ImportFromTextDocumentFile(filePath, takeFrom, takeTo);
        return new OkObjectResult(result);
    }

    [FunctionName("GetInformationOfHanzi")]
    public async Task<IActionResult> GetInformationOfHanzi(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "hanzi/{id}/hvdic")]
        HttpRequest req,
        string id,
        ILogger log)
    {
        var result = await _hanziService.GetHanziInformationFromHvDic(id);
        return new OkObjectResult(result);
    }

    [FunctionName("GetInformationOfMultipleHanzis")]
    [OpenApiOperation("apikey.query", new[] { "apikey" }, Summary = "API Key authentication code flow via querystring",
        Description = "This shows the API Key authentication code flow via querystring",
        Visibility = OpenApiVisibilityType.Important)]
    [OpenApiSecurity("apikeyquery_auth",
        SecuritySchemeType.ApiKey,
        In = OpenApiSecurityLocationType.Query,
        Name = "token")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(Dictionary<string, string>),
        Summary = "successful operation", Description = "successful operation")]
    public async Task<IActionResult> GetInformationOfMultipleHanzis(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "hanzi/multi/{listId}/hvdic")]
        HttpRequest req,
        string listId,
        ILogger log)
    {
        var queries = req.Query.ToDictionary(q => q.Key, q => (string)q.Value);
        var chars = listId.ToCharArray().Select(x => x.ToString());

        var result = chars.Select(async c => await _hanziService.GetHanziInformationFromHvDic(c))
            .Select(t => t.Result)
            .Where(i => i != null)
            .ToList();

        return new OkObjectResult(result);
    }

    [FunctionName("GetHanziListFromDb")]
    [QueryStringParameter("skip", "skip number of chars", DataType = typeof(int), Required = false)]
    [QueryStringParameter("take", "take number of chars", DataType = typeof(int), Required = false)]
    public async Task<IActionResult> GetHanziListFromDb(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "hanzi/db")]
        HttpRequest req,
        ILogger log)
    {
        List<Hanzi> result;
        var skip = int.Parse(req.Query["skip"]);
        var take = int.Parse(req.Query["take"]);
        if (skip == 0 || take == 0)
        {
            var allInDb = await _hanziService.GetAllInDb();
            result = allInDb.Any() ? allInDb.ToList() : null;
        }
        else
        {
            var allInDb2 = await _hanziService.GetAllInDb(skip, take);
            result = allInDb2.Any() ? allInDb2.ToList() : null;
        }

        return new OkObjectResult(result);
    }

    [FunctionName("RemoveSingleHanziFromDb")]
    public async Task<IActionResult> RemoveSingleHanziFromDb(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "hanzi/db/{id}")]
        HttpRequest req,
        string id,
        ILogger log)
    {
        var result = await _hanziService.Delete(id);
        return new OkObjectResult(result);
    }

    [FunctionName("GetMissingHanzis")]
    [QueryStringParameter("skip", "Number of chars needs translating", DataType = typeof(int), Required = false)]
    [QueryStringParameter("take", "Number of chars needs translating", DataType = typeof(int), Required = false)]
    public async Task<IActionResult> GetMissingHanzis(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "hanzi/missing/{filePath}")]
        HttpRequest req,
        string filePath,
        ILogger log)
    {
        var skip = int.Parse(req.Query["skip"]);
        var take = int.Parse(req.Query["take"]);
        var result = await _hanziService.FindMissingIds(filePath, skip, take);
        return new OkObjectResult(result);
    }

    [FunctionName("GetRandomListOfHanzis")]
    [QueryStringParameter("numberOfChars", "take number of chars", DataType = typeof(int), Required = true)]
    public async Task<IActionResult> GetRandomHanzis(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "hanzi/random")]
        HttpRequest req,
        ILogger log)
    {
        List<Hanzi> result;
        var numberOfChars = int.Parse(req.Query["numberOfChars"]);

        if (numberOfChars == 0) return new OkObjectResult("Please input numberOfChars");

        var fromDb = await _hanziService.GetRandomList(numberOfChars);
        var hanzis = fromDb.ToList();
        result = hanzis.Any() ? hanzis.ToList() : null;

        return new OkObjectResult(result);
    }
}
