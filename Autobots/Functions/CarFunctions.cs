using System.Linq;
using System.Threading.Tasks;
using Crawler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Repositories.UnitOfWork.Abstractions;

namespace Autobots.Functions;

public class CarFunctions
{
    private readonly IUnitOfWork _unitOfWork;

    public CarFunctions(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [ApiExplorerSettings(GroupName = "Cars")]
    [FunctionName("GetCars")]
    public async Task<IActionResult> GetElectricPrices(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "cars")] HttpRequest req,
        ILogger log)
    {
        var results = await _unitOfWork.Cars.All();
        return new OkObjectResult(results);
    }
}