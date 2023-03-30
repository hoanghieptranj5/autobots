namespace HanziCollector.Abstraction;

internal interface ITextDocumentReader
{
    string Read(string filePath);
    IEnumerable<string> ReadToCharArray(string filePath);
}