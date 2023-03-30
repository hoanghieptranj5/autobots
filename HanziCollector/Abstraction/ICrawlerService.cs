using HanziCollector.Models;

namespace HanziCollector.Abstraction;

internal interface ICrawlerService
{
    Task<IEnumerable<string>> Crawl(string website);
    Task<HanziFromHvDic> CrawlSingle(string hanzi);
}