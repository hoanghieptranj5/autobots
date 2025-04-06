using CosmosRepository.Abstractions;
using CosmosRepository.Clients;
using CosmosRepository.Entities.HanziCollector;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace CosmosRepository.Implementations;

public class HanziRepository(CosmosDbContext cosmosDbContext, string containerName, string partitionKeyPath)
    : Repository<Hanzi>(cosmosDbContext, containerName, partitionKeyPath), IHanziRepository<Hanzi>
{
    public new async Task<List<Hanzi>> GetRandomHanziList(int count)
    {
        var random = new Random();

        // Step 1: Pick one random bucket (partition)
        int bucket = random.Next(1, 11); // 1 to 10

        // Step 2: Fetch all items from that partition
        var allItems = new List<Hanzi>();

        var query = _container.GetItemLinqQueryable<Hanzi>(
                requestOptions: new QueryRequestOptions
                {
                    PartitionKey = new PartitionKey(bucket),
                    MaxItemCount = 300 // or more if needed
                })
            .ToFeedIterator();

        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            allItems.AddRange(response);
        }

        // Step 3: Shuffle and take 'count' items
        return allItems
            .OrderBy(_ => Guid.NewGuid())
            .Take(count)
            .ToList();
    }
}