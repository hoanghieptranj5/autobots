namespace HanziCollector.Abstraction;

public interface ICrawlerService
{
    Task<IEnumerable<string>> Crawl(string website);
}