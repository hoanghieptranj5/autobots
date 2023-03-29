namespace Repositories.UnitOfWork.Abstractions;

public interface IUnitOfWork
{
    ICarRepository Cars { get; set; }
    IElectricPriceRepository ElectricPrices { get; set; }
    IHanziRepository Hanzis { get; set; }
    
    Task CompleteAsync();
}