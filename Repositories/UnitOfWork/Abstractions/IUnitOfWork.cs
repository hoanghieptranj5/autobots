namespace Repositories.UnitOfWork.Abstractions;

public interface IUnitOfWork
{
    ICarRepository Cars { get; set; }
    IElectricPriceRepository ElectricPrices { get; set; }
    
    Task CompleteAsync();
}