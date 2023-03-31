using HanziCollector.Models;

namespace HanziCollector.Abstraction;

public interface IHanziService
{
    Task<IEnumerable<HanziFromHvDic>> ImportFromTextDocumentFile(string filePath, int takeFrom = 0, int takeTo = 100);
    Task<HanziFromHvDic> GetHanziInformationFromHvDic(string hanzi);
}