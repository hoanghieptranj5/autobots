using HtmlAgilityPack;

namespace Crawler.CollectorBase;

public class CollectorBase
{
    private readonly ICollector _collector;

    public CollectorBase(ICollector collector)
    {
        _collector = collector;
    }

    public async Task<IEnumerable<string>> Collect()
    {
        var httpClient = new HttpClient();
        var html = await httpClient.GetStringAsync(_collector.Url);
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        
        var values = (IEnumerable<string>) _collector.DoCollectBusiness(doc);
        return values;
    }
}