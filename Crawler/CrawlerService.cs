using HtmlAgilityPack;

namespace Crawler;

public static class CrawlerService
{
  public static async Task<IEnumerable<string>> Run()
  {
    var client = new HttpClient();

    var html = await client.GetStringAsync("http://dontcodetired.com/blog/archive");

    var doc = new HtmlDocument();
    doc.LoadHtml(html);

    var postTitles = doc.DocumentNode
      .Descendants("td")
      .Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value.Contains("title"))
      .Select(x => x.InnerText);

    return postTitles;
  }
}