using System;
using System.Reflection;
using Autobots;
using AzureFunctions.Extensions.Swashbuckle;
using AzureFunctions.Extensions.Swashbuckle.Settings;
using ElectricCalculator.Logics;
using ElectricCalculator.Profiles;
using HanziCollector.DI;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repositories.Models;
using Repositories.UnitOfWork;
using Repositories.UnitOfWork.Abstractions;
using Swashbuckle.AspNetCore.SwaggerGen;

[assembly: WebJobsStartup(typeof(Startup))]

namespace Autobots;

public class Startup : FunctionsStartup
{
  public override void Configure(IFunctionsHostBuilder builder)
  {
    builder.AddSwashBuckle(Assembly.GetExecutingAssembly(), opts =>
    {
      opts.AddCodeParameter = true;
      opts.Documents = new[]
      {
        new SwaggerDocument
        {
          Name = "v1",
          Title = "Swagger document",
          Description = "Integrate Swagger UI With Azure Functions",
          Version = "v2"
        }
      };
      opts.ConfigureSwaggerGen = x =>
      {
        x.CustomOperationIds(apiDesc =>
        {
          return apiDesc.TryGetMethodInfo(out MethodInfo mInfo) ? mInfo.Name : default(Guid).ToString();
        });
      };
    });

    builder.Services.AddLogging();

    var connectionString =
      Environment.GetEnvironmentVariable("SqlConnectionString", EnvironmentVariableTarget.Process);
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
      SqlServerDbContextOptionsExtensions.UseSqlServer(options, connectionString));

    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

    builder.Services.AddAutoMapper(typeof(ElectricPriceProfile));
    builder.Services.AddScoped<IElectricPriceRepository, ElectricPriceRepository>();
    builder.Services.AddScoped<IElectricPriceService, ElectricPriceService>();
    builder.Services.AddScoped<ICalculationLogic, CalculationLogic>();

    builder.Services.SetupHanziDependencies();
  }
}