using ElectricCalculator.Logics;
using Microsoft.Extensions.DependencyInjection;


namespace ElectricCalculator.DI;

public static class SetupElectricityExtension
{
    public static void SetupElectricityPriceDependencies(this IServiceCollection services)
    {
        services.AddScoped<IElectricPriceService, ElectricPriceService>();
        services.AddScoped<ICalculationLogic, CalculationLogic>();
    }
}
