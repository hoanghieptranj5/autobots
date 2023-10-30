using HanziCollector.Abstraction;
using HanziCollector.Models;
using HtmlAgilityPack;

namespace HanziCollector.Implementations;

public class CrawlerService : ICrawlerService
{
    private string WebSite(string x)
    {
        return $"https://hvdic.thivien.net/whv/{x}";
    }

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

    public async Task<HanziFromHvDic> CrawlSingle(string hanzi)
    {
        var client = new HttpClient();

        var html = await client.GetStringAsync(WebSite(hanzi));

        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var pinYins = doc.DocumentNode
            .Descendants("a")
            .Where(x => x.Attributes.Contains("href") && x.Attributes["href"].Value.Contains("/py/"))
            .Select(x => x.InnerText);

        var hanViets = doc.DocumentNode
            .Descendants("span")
            .Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value.Contains("hvres-goto-link"))
            .Select(x => x.InnerText);

        var cantoneses = doc.DocumentNode
            .Descendants("a")
            .Where(x => x.Attributes.Contains("href") && x.Attributes["href"].Value.Contains("/reading/1/4/"))
            .Select(x => x.InnerText);

        var meanings = doc.DocumentNode
            .Descendants("div")
            .Where(x => x.Attributes.Contains("class") &&
                        x.Attributes["class"].Value.Contains("hvres-meaning han-clickable"))
            .Select(x => x.InnerText);


        return new HanziFromHvDic
        {
            Id = hanzi,
            Pinyin = string.Join(", ", pinYins.ToArray()),
            HanViet = string.Join(", ", hanViets.ToArray()),
            Cantonese = string.Join(", ", cantoneses.ToArray()),
            MeaningInVietnamese = string.Join(", ", meanings.ToArray())
        };
    }
}
