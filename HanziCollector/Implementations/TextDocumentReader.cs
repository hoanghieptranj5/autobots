using HanziCollector.Abstraction;

namespace HanziCollector.Implementations;

public class TextDocumentReader : ITextDocumentReader
{
    public string Read(string filePath)
    {
        string allTexts;
        try
        {
            using var sr = new StreamReader(filePath);
            allTexts = sr.ReadToEnd();
            Console.WriteLine(allTexts);
        }
        catch (IOException e)
        {
            Console.WriteLine(e);
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unknown Exception: {e.Message}");
            throw;
        }

        return allTexts;
    }

    public IEnumerable<string> ReadToCharArray(string filePath)
    {
        IEnumerable<string> results;
        try
        {
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
        }
        catch (IOException e)
        {
            Console.WriteLine(e);
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unknown Exception: {e.Message}");
            throw;
        }

        return results;
    }
}