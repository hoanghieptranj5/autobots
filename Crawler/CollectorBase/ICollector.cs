using HtmlAgilityPack;

namespace Crawler.CollectorBase;

public interface ICollector<T> where T : class
{
  string Url { get; }
  IEnumerable<T> DoCollectBusiness(HtmlDocument document);
}