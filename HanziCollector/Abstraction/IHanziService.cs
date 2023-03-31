using System.Collections;
using HanziCollector.Models;
using Repositories.Models.HanziCollector;

namespace HanziCollector.Abstraction;

public interface IHanziService
{
    Task<IEnumerable<HanziFromHvDic>> ImportFromTextDocumentFile(string filePath, int takeFrom = 0, int takeTo = 100);
    Task<HanziFromHvDic> GetHanziInformationFromHvDic(string hanzi);
    Task<Hanzi?> GetSingle(string id);
    Task<IEnumerable<Hanzi>> GetAllInDb();
    Task<bool> Delete(string id);
}