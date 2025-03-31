using CosmosRepository.Entities.HanziCollector;
using Microsoft.Azure.Cosmos;

namespace DatabaseMigration;

public class HanziBucketMigrator
{
    private readonly Container _sourceContainer; // Old: /id partitioned
    private readonly Container _targetContainer; // New: /Bucket partitioned
    private readonly int _bucketCount;

    public HanziBucketMigrator(Container source, Container target, int bucketCount = 10)
    {
        _sourceContainer = source;
        _targetContainer = target;
        _bucketCount = bucketCount;
    }

    public async Task RunMigrationAsync()
    {
        Console.WriteLine("Starting migration...");

        var query = _sourceContainer.GetItemQueryIterator<Hanzi>(
            new QueryDefinition("SELECT * FROM c"),
            requestOptions: new QueryRequestOptions { MaxItemCount = 100 });

        int totalMigrated = 0;
        while (query.HasMoreResults)
        {
            FeedResponse<Hanzi> response = await query.ReadNextAsync();

            foreach (var hanzi in response)
            {
                // Assign a bucket value (deterministic or random)
                hanzi.Bucket = (hanzi.InsertedOrder % _bucketCount) + 1;

                try
                {
                    await _targetContainer.CreateItemAsync(
                        hanzi,
                        new PartitionKey(hanzi.Bucket));

                    totalMigrated++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Failed to insert {hanzi.Id}: {ex.GetHashCode()} - {ex.Message}");
                }
            }
        }

        Console.WriteLine($"✅ Migration completed: {totalMigrated} documents migrated.");
    }
}