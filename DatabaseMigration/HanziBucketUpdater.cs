using CosmosRepository.Entities.HanziCollector;
using Microsoft.Azure.Cosmos;

namespace DatabaseMigration;

public class HanziBucketUpdater
{
    private readonly Container _container;
    private readonly int _bucketCount;

    public HanziBucketUpdater(Container container, int bucketCount = 10)
    {
        _container = container;
        _bucketCount = bucketCount;
    }

    public async Task UpdateAllBucketsAsync()
    {
        Console.WriteLine("🔄 Starting update of Bucket fields...");

        var query = _container.GetItemQueryIterator<Hanzi>(
            new QueryDefinition("SELECT * FROM c"),
            requestOptions: new QueryRequestOptions { MaxItemCount = 100 });

        int updatedCount = 0;

        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            foreach (var hanzi in response)
            {
                // Assign a bucket deterministically
                hanzi.Bucket = (hanzi.InsertedOrder % _bucketCount) + 1;

                try
                {
                    // Use Bucket as the new partition key
                    await _container.ReplaceItemAsync(
                        hanzi,
                        hanzi.Id,
                        new PartitionKey(hanzi.Bucket));

                    updatedCount++;
                }
                catch (CosmosException ex)
                {
                    Console.WriteLine($"❌ Failed to update '{hanzi.Id}': {ex.StatusCode} - {ex.Message}");
                }
            }
        }

        Console.WriteLine($"✅ Bucket field updated for {updatedCount} documents.");
    }
}
