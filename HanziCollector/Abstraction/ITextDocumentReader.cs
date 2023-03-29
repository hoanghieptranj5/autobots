namespace HanziCollector.Abstraction;

public interface ITextDocumentReader
{
    string Read(string filePath);
    IEnumerable<string> ReadToCharArray(string filePath);
}