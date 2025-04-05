using CosmosRepository.Abstractions;
using CosmosRepository.Entities.ElectricCalculator;

namespace ElectricCalculator.Logics;

public class ElectricPriceService : IElectricPriceService
{
    private readonly IUnitOfWork _unitOfWork;

    public ElectricPriceService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ElectricPrice?> GetSingle(int id)
    {
        return await _unitOfWork.ElectricPrices.GetById(id.ToString());
    }

    public async Task<IEnumerable<ElectricPrice>> GetAll()
    {
        return await _unitOfWork.ElectricPrices.All();
    }

    public async Task<bool> InsertSingle(ElectricPrice e)
    {
        var result = await _unitOfWork.ElectricPrices.Add(e);
        return result;
    }

    public async Task<bool> RemoveSingle(int id)
    {
        var result = await _unitOfWork.ElectricPrices.Delete(id.ToString());
        return result;
    }

    public async Task<bool> UpdateSingle(ElectricPrice e)
    {
        var result = await _unitOfWork.ElectricPrices.Add(e);
        return result;
    }
}
