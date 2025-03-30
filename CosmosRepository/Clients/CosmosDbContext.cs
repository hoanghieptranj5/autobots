using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace CosmosRepository.Clients;

public class CosmosDbContext
{
    private readonly CosmosClient _client;
    private readonly Database _database;
    
    public CosmosDbContext(IConfiguration config)
    {
        _client = new CosmosClient(config["CosmosDb:Account"], config["CosmosDb:Key"]);
        _database = _client.CreateDatabaseIfNotExistsAsync(config["CosmosDb:DatabaseName"]).GetAwaiter().GetResult();
    }

    public Container GetContainer(string containerName)
    {
        return _database.CreateContainerIfNotExistsAsync(containerName, "/id").GetAwaiter().GetResult();
    }
}