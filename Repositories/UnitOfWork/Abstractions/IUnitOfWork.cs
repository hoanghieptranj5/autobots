namespace Repositories.UnitOfWork.Abstractions;

public interface IUnitOfWork
{
    IElectricPriceRepository ElectricPrices { get; set; }
    IHanziRepository Hanzis { get; set; }
    IUserRepository Users { get; set; }

    Task CompleteAsync();
}
