using HanziCollector.Abstraction;
using Microsoft.EntityFrameworkCore;
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
    var result = await _unitOfWork.Hanzis
      .AllQuery()
      .OrderBy(x => x.Id)
      .Take(take)
      .Skip(skip)
      .ToListAsync();
    return result;
  }

  public Task<IEnumerable<Hanzi>> ReadRandomHanziList()
  {
    throw new NotImplementedException();
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