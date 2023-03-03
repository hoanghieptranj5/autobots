using Crawler.CollectorBase;
using HtmlAgilityPack;
using Repositories.Models;

namespace Crawler.ConcreteCollectors;

public class ChototxeCollector : ICollector<Car>
{
    public string Url { get; } =
        "https://xe.chotot.com/mua-ban-oto-honda-civic-tp-ho-chi-minh-sdcb6cm241?sp=1&price=0-405000000";

    private readonly string RowDataXpath = "//*[@class='AdItem_wrapperAdItem__S6qPH  AdItem_big__70CJq']";
    
    public IEnumerable<Car> DoCollectBusiness(HtmlDocument document)
    {
        var rows =
            document.DocumentNode.SelectNodes(RowDataXpath);

        var result = new List<Car>();
        var count = 0;
        foreach (var row in rows)
        {
            var name = row.SelectSingleNode(".//*[@class='commonStyle_adTitle__g520j ']");
            var price = row.SelectSingleNode(".//*[@class='AdBody_adItemPrice__Xr5sP']");
            
            var car = new Car
            {
                CollectedDate = DateTime.Now,
                CollectedFrom = Url,
                Description = price.InnerText,
                Id = count,
                Name = name.InnerText
            };
            result.Add(car);
            
            count++;
        }
        
        return result;
    }
}