using HanziCollector.Abstraction;
using Repositories.Models.HanziCollector;
using Repositories.UnitOfWork.Abstractions;

namespace HanziCollector.Implementations;

public class HanziDbService : IHanziDbService
{
    private readonly IUnitOfWork _unitOfWork;

    public HanziDbService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> SaveSingle(Hanzi hanzi)
    {
        var completed = await _unitOfWork.Hanzis.Add(hanzi);
        await _unitOfWork.CompleteAsync();
        return completed;
    }

    public async Task<IEnumerable<Hanzi>> ReadAll()
    {
        var hanzis = await _unitOfWork.Hanzis.All();
        return hanzis;
    }

    public async Task<bool> DeleteSingle(string id)
    {
        var completed = await _unitOfWork.Hanzis.Delete(id);
        await _unitOfWork.CompleteAsync();
        return completed;
    }

    public Task<bool> UpdateSingle(Hanzi hanzi)
    {
        throw new NotImplementedException();
    }
}