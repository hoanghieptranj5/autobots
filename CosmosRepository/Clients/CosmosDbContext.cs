using CosmosRepository.Settings;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace CosmosRepository.Clients;

public class CosmosDbContext
{
    private readonly CosmosClient _client;
    private readonly Database _database;

    public CosmosDbContext(IOptions<CosmosDbSettings> options)
    {
        var config = options.Value;

        if (string.IsNullOrEmpty(config.Key))
        {
            throw new ArgumentNullException(nameof(config.Key), "CosmosDb key is missing.");
        }

        _client = new CosmosClient(config.Account, config.Key);
        _database = _client.CreateDatabaseIfNotExistsAsync(config.DatabaseName).GetAwaiter().GetResult();
    }

    public Container GetContainer(string containerName)
    {
        return _database.CreateContainerIfNotExistsAsync(containerName, "/Id").GetAwaiter().GetResult();
    }
}