using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;

namespace CosmosRepository.Entities.Vocabulary;

public class Vocabulary : CosmosEntity
{
    [JsonProperty("id")]
    public string Id { get; set; }
    public string Word { get; set; }
    public string Category { get; set; }
    public string Example { get; set; }
    public string Meaning { get; set; }
    public string Cantonese { get; set; }
    public string Pinyin { get; set; }
    public PartitionKey GetPartitionKey()
    {
        return new PartitionKey(this.Id);
    }
}
