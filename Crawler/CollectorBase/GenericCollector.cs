using HtmlAgilityPack;

namespace Crawler.CollectorBase;

public class GenericCollector<T> where T : class
{
    private readonly ICollector<T> _collector;

    public GenericCollector(ICollector<T> collector)
    {
        _collector = collector;
    }

    public async Task<IEnumerable<T>> Collect()
    {
        var httpClient = new HttpClient();
        var html = await httpClient.GetStringAsync(_collector.Url);
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        
        var values = _collector.DoCollectBusiness(doc);
        return values;
    }
}