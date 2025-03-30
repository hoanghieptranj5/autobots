namespace CosmosRepository.Entities.ElectricCalculator;

public class ElectricPrice : BaseEntity
{
    public int From { get; set; }
    public int To { get; set; }
    public float StandardPrice { get; set; }
    public float Price { get; set; }
    public float Usage { get; set; }
}
