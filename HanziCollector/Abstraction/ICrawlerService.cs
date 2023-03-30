namespace HanziCollector.Abstraction;

internal interface ICrawlerService
{
    Task<IEnumerable<string>> Crawl(string website);
}