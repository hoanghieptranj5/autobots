using HanziCollector.Models;

namespace HanziCollector.Abstraction;

public interface IHanziService
{
    Task ImportFromTextDocumentFile(string filePath);
    Task<HanziFromHvDic> GetHanziInformationFromHvDic(string hanzi);
}