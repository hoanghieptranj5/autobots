using HanziCollector.Models;
using Repositories.Models.HanziCollector;

namespace HanziCollector.Abstraction;

public interface IHanziService
{
    Task<IEnumerable<HanziFromHvDic>> ImportFromTextDocumentFile(string filePath, int takeFrom = 0, int takeTo = 100);
    Task<HanziFromHvDic> GetHanziInformationFromHvDic(string hanzi);
    Task<Hanzi?> GetSingle(string id);
    Task<IEnumerable<Hanzi>> GetAllInDb();
    Task<IEnumerable<Hanzi>> GetAllInDb(int skip, int take);
    Task<IEnumerable<Hanzi>> GetRandomList(int length, int min = 1, int max = 3000);
    Task<bool> Delete(string id);
    Task<IEnumerable<string>> FindMissingIds(string filePath, int skip, int take);
}
