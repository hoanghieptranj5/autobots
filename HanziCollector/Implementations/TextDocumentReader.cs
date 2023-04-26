using HanziCollector.Abstraction;

namespace HanziCollector.Implementations;

public class TextDocumentReader : ITextDocumentReader
{
  public string Read(string filePath)
  {
    using var sr = new StreamReader(filePath);
    var allTexts = sr.ReadToEnd();
    Console.WriteLine(allTexts);
    return allTexts;
  }

  public IEnumerable<string> ReadToCharArray(string filePath)
  {
    IEnumerable<string> results;

    using var sr = new StreamReader(filePath);
    var allTexts = sr.ReadToEnd();

    if (string.IsNullOrEmpty(allTexts))
    {
      throw new Exception("No data found");
    }

    var chars = allTexts.ToCharArray().ToList();
    results = chars.Where(c1 => !char.IsWhiteSpace(c1))
      .Select(c2 => c2.ToString())
      .ToList();
    return results;
  }
}