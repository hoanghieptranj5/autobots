using Crawler.CollectorBase;
using HtmlAgilityPack;

namespace Crawler.ConcreteCollectors;

public class GoldPriceCollector : ICollector
{
    public string Url { get; private set; } = "https://www.24h.com.vn/gia-vang-hom-nay-c425.html";

    public object DoCollectBusiness(HtmlDocument document)
    {
        var testValues =
            document.DocumentNode.SelectSingleNode("//*[@id='container_tin_gia_vang']//tr[@data-seach='sjc_tp_hcm']");

        var values = testValues.ChildNodes.Select(x => x.InnerText);
        return values;
    }
}