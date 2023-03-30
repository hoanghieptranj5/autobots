using HanziCollector.Abstraction;

namespace HanziCollector.Implementations;

internal class HanziService : IHanziService
{
    private readonly ITextDocumentReader _textDocumentReader;
    private readonly ICrawlerService _crawler;

    public HanziService(ITextDocumentReader textDocumentReader, ICrawlerService crawlerService)
    {
        _textDocumentReader = textDocumentReader;
        _crawler = crawlerService;
    }

    public async Task ImportFromTextDocumentFile(string filePath)
    {
        try
        {
            var characters = _textDocumentReader.ReadToCharArray(filePath);
            var firstChar = characters.First();
            var hanViet = await _crawler.Crawl($"https://hvdic.thivien.net/whv/{firstChar}");
            Console.WriteLine(hanViet);
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
    }
}