using ElectricCalculator.Logics;
using Microsoft.Extensions.DependencyInjection;
using Repositories.UnitOfWork;
using Repositories.UnitOfWork.Abstractions;

namespace ElectricCalculator.DI;

public static class SetupElectricityExtension
{
  public static void SetupElectricityPriceDependencies(this IServiceCollection services)
  {
    services.AddScoped<IElectricPriceRepository, ElectricPriceRepository>();
    services.AddScoped<IElectricPriceService, ElectricPriceService>();

    services.AddScoped<ICalculationLogic, CalculationLogic>();
  }
}