﻿using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using ElectricCalculator.Logics;
using ElectricCalculator.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Repositories.Models.ElectricCalculator;
using Repositories.UnitOfWork;

namespace Autobots.Functions;

public class ElectricCalculatorFunctions
{
    private readonly ICalculationLogic _calculationLogic;
    private readonly IElectricPriceService _electricPriceService;
    private readonly IMapper _mapper;

    public ElectricCalculatorFunctions(ICalculationLogic calculationLogic, IElectricPriceService electricPriceService,
        IMapper mapper)
    {
        _calculationLogic = calculationLogic;
        _electricPriceService = electricPriceService;
        _mapper = mapper;
    }

    [ApiExplorerSettings(GroupName = "ElectricCalculator")]
    [FunctionName("GetElectricPrices")]
    public async Task<IActionResult> GetElectricPrices(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "electricPrices")]
        HttpRequest req,
        ILogger log)
    {
        var results = await _electricPriceService.GetAll();
        return new OkObjectResult(results);
    }

    [ApiExplorerSettings(GroupName = "ElectricCalculator")]
    [FunctionName("SaveElectricPrice")]
    public async Task<IActionResult> SaveElectricPrice(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "electricPrices")]
        [RequestBodyType(typeof(CreateElectricPriceRequestModel), "Create Type")]
        HttpRequest req,
        ILogger log)
    {
        var requestObject = await new StreamReader(req.Body).ReadToEndAsync();
        var requestModel = JsonConvert.DeserializeObject<CreateElectricPriceRequestModel>(requestObject);
        var results = await _electricPriceService.InsertSingle(_mapper.Map<ElectricPrice>(requestModel));
        return new OkObjectResult(results);
    }

    [ApiExplorerSettings(GroupName = "ElectricCalculator")]
    [FunctionName("DeleteElectricPrice")]
    public async Task<IActionResult> DeleteElectricPrice(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "electricPrices/{priceId}")]
        HttpRequest req,
        int priceId,
        ILogger log)
    {
        var results = await _electricPriceService.RemoveSingle(priceId);
        return new OkObjectResult(results);
    }

    [ApiExplorerSettings(GroupName = "ElectricCalculator")]
    [FunctionName("CalculateUsage")]
    public async Task<IActionResult> CalculateUsage(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "electricPrices/usage/{value}")]
        HttpRequest req,
        int value,
        ILogger log)
    {
        var result = await _calculationLogic.CalculateAsync(value);
        return new OkObjectResult(result);
    }
}
