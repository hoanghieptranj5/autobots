using CosmosRepository.Entities.HanziCollector;
using HanziCollector.Models;

namespace HanziCollector.Abstraction;

public interface IHanziService
{
    Task<HanziFromHvDic> GetHanziInformationFromHvDic(string hanzi);
    Task<Hanzi?> GetSingle(string id);
    Task<IEnumerable<Hanzi>> GetAllInDb();
    Task<IEnumerable<Hanzi>> GetAllInDb(int skip, int take);
    Task<IEnumerable<Hanzi>> GetRandomList(int length, int min = 1, int max = 3000);
    Task<bool> Delete(string id);
    Task<IEnumerable<string>> FindMissingIds(string filePath, int skip, int take);
}
