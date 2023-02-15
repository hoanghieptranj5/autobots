using ElectricCalculator.Models;

namespace ElectricCalculator.Logics;

public interface ICalculationLogic
{
    Task<CalculatedModel> CalculateAsync(int usage);
}