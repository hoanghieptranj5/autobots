using DatabaseMigration;
using Microsoft.Azure.Cosmos;

class Program
{
    public static async Task Main(string[] args)
    {
        var cosmosClient = new CosmosClient("XXX", "XXXX");

        var database = cosmosClient.GetDatabase("XX");

        var sourceContainer = database.GetContainer("XX");   // PartitionKey: /id
        var targetContainer = database.GetContainer("XX");   // PartitionKey: /Bucket

        var migrator = new HanziBucketMigrator(sourceContainer, targetContainer);

        await migrator.RunMigrationAsync();
    }
}