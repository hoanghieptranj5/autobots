using ElectricCalculator.Models;
using Repositories.Models.ElectricCalculator;
using Repositories.UnitOfWork.Abstractions;

namespace ElectricCalculator.Logics;

public class CalculationLogic : ICalculationLogic
{
    private readonly IUnitOfWork _unitOfWork;

    public CalculationLogic(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CalculatedModel> CalculateAsync(int usage)
    {
        var prices = await _unitOfWork.ElectricPrices.All();
        var remaining = usage;
        var total = 0.0f;
        var results = new CalculatedModel
        {
            Items = new List<ElectricPrice>()
        };

        foreach (var pricing in prices)
            if (remaining >= pricing.To - pricing.From)
            {
                remaining -= pricing.To - pricing.From;
                total += pricing.StandardPrice * (pricing.To - pricing.From);

                results.Items.Add(new ElectricPrice
                {
                    From = pricing.From,
                    To = pricing.To,
                    StandardPrice = pricing.StandardPrice,
                    Price = pricing.StandardPrice * (pricing.To - pricing.From),
                    Usage = pricing.To - pricing.From
                });
                Console.WriteLine(
                    $"From {pricing.From} to {pricing.To}: {pricing.StandardPrice} * {pricing.To - pricing.From} = {pricing.StandardPrice * (pricing.To - pricing.From)}");
            }
            else
            {
                total += pricing.StandardPrice * remaining;

                results.Items.Add(new ElectricPrice
                {
                    From = pricing.From,
                    To = pricing.To,
                    StandardPrice = pricing.StandardPrice,
                    Price = pricing.StandardPrice * remaining,
                    Usage = remaining
                });

                Console.WriteLine(
                    $"From {pricing.From} to {pricing.To}: {pricing.StandardPrice} * {remaining} = {pricing.StandardPrice * remaining}");
                remaining = 0;
            }

        results.Usage = usage;
        results.Total = total;

        return results;
    }
}
