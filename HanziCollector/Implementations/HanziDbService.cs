using CosmosRepository.Contracts;
using CosmosRepository.Entities.HanziCollector;
using HanziCollector.Abstraction;
using Microsoft.Azure.Cosmos;

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
        await _unitOfWork.SaveChangesAsync();
        return completed;
    }

    /// <summary>
    /// Not recommended for use.
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<Hanzi>> ReadAll()
    {
        var hanzis = await _unitOfWork.Hanzis.All();
        return hanzis;
    }

    public async Task<IEnumerable<Hanzi>> ReadRange(int skip, int take)
    {
        var result = _unitOfWork.Hanzis
            .AllQuery()
            .Skip(skip)
            .Take(take);
        return result.ToList();
    }

    public async Task<IEnumerable<Hanzi>> ReadRandomHanziList(int count = 20)
    {
        // Call your existing method with the random list
        return await _unitOfWork.Hanzis.GetRandomHanziList(count);
    }

    public async Task<bool> DeleteSingle(string id)
    {
        var completed = await _unitOfWork.Hanzis.Delete(id);
        await _unitOfWork.SaveChangesAsync();
        return completed;
    }

    public Task<bool> UpdateSingle(Hanzi hanzi)
    {
        throw new NotImplementedException();
    }
}
