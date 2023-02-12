using HtmlAgilityPack;

namespace Crawler.CollectorBase;

public interface ICollector
{
    string Url { get; }
    object DoCollectBusiness(HtmlDocument document);
}