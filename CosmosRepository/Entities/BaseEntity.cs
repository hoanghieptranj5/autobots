using Newtonsoft.Json;

namespace CosmosRepository.Entities;

public class BaseEntity
{
    [JsonProperty("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();
}