using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;

namespace CosmosRepository.Entities.HanziCollector;

public class Hanzi : CosmosEntity
{
    [JsonProperty("id")]
    public string Id { get; set; }
    public string? HanViet { get; set; }
    public string? Pinyin { get; set; }
    public string? Cantonese { get; set; }
    public int? Stroke { get; set; }
    public string? MeaningInVietnamese { get; set; }

    /// <summary>
    /// Indexed field, used for quickly randomly access a Hanzi Character
    /// </summary>
    public int InsertedOrder { get; set; }

    public int Bucket { get; set; }
    public PartitionKey GetPartitionKey()
    {
        return new PartitionKey(Bucket);
    }
}
