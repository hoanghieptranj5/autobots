using HtmlAgilityPack;

namespace Crawler.GoldPriceCollector;

public static class GoldCollector
{
    private const string Website24h = "https://www.24h.com.vn/gia-vang-hom-nay-c425.html";

    public static async Task<IEnumerable<string>> Collect()
    {
        var client = new HttpClient();

        var html = await client.GetStringAsync(Website24h);

        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var testValues =
            doc.DocumentNode.SelectSingleNode("//*[@id='container_tin_gia_vang']//tr[@data-seach='sjc_tp_hcm']");

        var values = testValues.ChildNodes.Select(x => x.InnerText);

        return values;
    }
}