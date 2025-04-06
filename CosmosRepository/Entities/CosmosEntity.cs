using Microsoft.Azure.Cosmos;

namespace CosmosRepository.Entities;

public interface CosmosEntity
{
    public string Id { get; set; }
    public PartitionKey GetPartitionKey();
}