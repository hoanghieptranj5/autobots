using Repositories.Models.ElectricCalculator;

namespace Repositories.UnitOfWork.Abstractions;

public interface IElectricPriceRepository : IRepository<ElectricPrice, int>
{
}