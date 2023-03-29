using HanziCollector.Abstraction;
using HtmlAgilityPack;

namespace HanziCollector.Implementations;

public class CrawlerService : ICrawlerService
{
    public async Task<IEnumerable<string>> Crawl(string website)
    {
        var client = new HttpClient();

        var html = await client.GetStringAsync(website);

        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var postTitles = doc.DocumentNode
            .Descendants("span")
            .Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value.Contains("hvres-goto-link"))
            .Select(x => x.InnerText);

        return postTitles;
    }
}