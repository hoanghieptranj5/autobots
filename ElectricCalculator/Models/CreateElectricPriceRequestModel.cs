using Newtonsoft.Json;

namespace ElectricCalculator.Models;

public class CreateElectricPriceRequestModel
{
    [JsonProperty("from")]
    public int From { get; set; }
    [JsonProperty("to")] 
    public int To { get; set; }
    [JsonProperty("standardPrice")]
    public long StandardPrice { get; set; }
    [JsonProperty("description")]
    public string Description { get; set; }
}