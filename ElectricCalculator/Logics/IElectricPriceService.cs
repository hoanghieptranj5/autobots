using CosmosRepository.Entities.ElectricCalculator;

namespace ElectricCalculator.Logics;

public interface IElectricPriceService
{
    Task<ElectricPrice?> GetSingle(int id);
    Task<IEnumerable<ElectricPrice>> GetAll();
    Task<bool> InsertSingle(ElectricPrice e);
    Task<bool> RemoveSingle(int id);
    Task<bool> UpdateSingle(ElectricPrice e);
}
