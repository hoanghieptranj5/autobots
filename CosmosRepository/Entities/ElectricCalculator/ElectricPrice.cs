using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;

namespace CosmosRepository.Entities.ElectricCalculator;

public class ElectricPrice : CosmosEntity
{
    [JsonProperty("id")]
    public string Id { get; set; }

    public int From { get; set; }
    public int To { get; set; }
    public float StandardPrice { get; set; }
    public float Price { get; set; }
    public float Usage { get; set; }
    
    public PartitionKey GetPartitionKey()
    {
        return new PartitionKey(Id);
    }
}
