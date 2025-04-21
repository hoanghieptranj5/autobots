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
        var prices = await _unitOfWork.ElectricPrices.All();
        return prices.ToList().OrderBy(x => x.From);
    }

    public async Task<bool> InsertSingle(ElectricPrice e)
    {
        var result = await _unitOfWork.ElectricPrices.Add(e);
        return result;
    }

    public async Task<bool> RemoveSingle(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateSingle(ElectricPrice e)
    {
        var result = await _unitOfWork.ElectricPrices.Add(e);
        return result;
    }
}
