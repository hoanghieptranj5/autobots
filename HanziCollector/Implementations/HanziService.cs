using HanziCollector.Abstraction;
using HanziCollector.Models;

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

    public async Task<IEnumerable<HanziFromHvDic>> ImportFromTextDocumentFile(string filePath, int takeFrom = 0, int takeTo = 100)
    {
        List<HanziFromHvDic> result;
        try
        {
            var characters = _textDocumentReader.ReadToCharArray(filePath);
            var chunk = characters.Skip(takeFrom).Take(takeTo).ToList();
            result = chunk.Select(async c => await _crawler.CrawlSingle(c))
                .Select(t => t.Result)
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

        return result;
    }

    public async Task<HanziFromHvDic> GetHanziInformationFromHvDic(string hanzi)
    {
        var result = await _crawler.CrawlSingle(hanzi);
        return result;
    }
}