using System.Threading.Tasks;
using Crawler.CollectorBase;
using Crawler.ConcreteCollectors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Repositories.Models;

namespace Autobots.Functions;

public class CrawlerFunctions
{
  [ApiExplorerSettings(GroupName = "Crawler")]
  [FunctionName("TriggerCollection")]
  public async Task<IActionResult> TriggerCollection(
    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "crawlers")]
    HttpRequest req,
    ILogger log)
  {
    var collector = new GenericCollector<string>(new GoldPriceCollector());
    var results = await collector.Collect();
    return new OkObjectResult(results);
  }
}