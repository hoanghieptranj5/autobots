using Repositories.Models.ElectricCalculator;

namespace ElectricCalculator.Models;

public class CalculatedModel
{
    public IList<ElectricPrice> Items { get; set; }
    public float Usage { get; set; }
    public float Total { get; set; }

    public double TotalWithVAT => Total * 1.1;
}