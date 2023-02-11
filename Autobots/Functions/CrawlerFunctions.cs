using System.Linq;
using System.Threading.Tasks;
using Crawler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Autobots.Functions;

public class CrawlerFunctions
{
    [ApiExplorerSettings(GroupName = "Crawler")]
    [FunctionName("TriggerCollection")]
    public async Task<IActionResult> TriggerCollection(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "crawlers")] HttpRequest req,
        ILogger log)
    {
        var results = await CrawlerService.Run();
        return new OkObjectResult(results.Take(100));
    }
}