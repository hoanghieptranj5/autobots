namespace HanziCollector.Abstraction;

public interface IHanziService
{
    Task ImportFromTextDocumentFile(string filePath);
}