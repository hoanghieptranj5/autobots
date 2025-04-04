using System.Linq.Expressions;
using CosmosRepository.Clients;
using CosmosRepository.Contracts;
using CosmosRepository.Entities;
using CosmosRepository.Entities.HanziCollector;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace CosmosRepository.Implementations;

public class Repository<T, R> : IRepository<T, R> where T : BaseEntity
{
    private readonly Container _container;

    public Repository(CosmosDbContext cosmosDbContext, string containerName, string partitionKeyPath)
    {
        _container = cosmosDbContext.GetContainer(containerName, partitionKeyPath);
    }

    public async Task<IEnumerable<T>> All()
    {
        var query = _container.GetItemQueryIterator<T>("SELECT * FROM c");
        var results = new List<T>();

        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            results.AddRange(response);
        }

        return results;
    }

    public IQueryable<T> AllQuery()
    {
        // Cosmos DB doesn't support IQueryable directly — you'd need LINQ-to-Cosmos with EF Core or LINQ-to-Objects
        throw new NotSupportedException(
            "Cosmos DB does not support IQueryable directly. Use Find with expressions instead.");
    }

    public async Task<T?> GetById(R id)
    {
        try
        {
            var response = await _container.ReadItemAsync<T>(id!.ToString(), new PartitionKey(id.ToString()));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<bool> Add(T entity)
    {
        try
        {
            await _container.CreateItemAsync(entity, new PartitionKey(entity.Id));
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> Delete(R id)
    {
        try
        {
            await _container.DeleteItemAsync<T>(id!.ToString(), new PartitionKey(id.ToString()));
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> Upsert(T entity)
    {
        try
        {
            await _container.UpsertItemAsync(entity, new PartitionKey(entity.Id));
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
    {
        var query = _container.GetItemLinqQueryable<T>(allowSynchronousQueryExecution: false)
            .Where(predicate)
            .ToFeedIterator();

        var results = new List<T>();

        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            results.AddRange(response);
        }

        return results;
    }

    public async Task<IEnumerable<T>> SelectIn(List<int> secondaryFieldIds)
    {
        // Split the list into comma-separated values
        var idsParam = string.Join(", ", secondaryFieldIds);

        // TODO: need to replace this hardcoded in the future
        var queryText = $"SELECT * FROM c WHERE c.InsertedOrder IN ({idsParam})";

        var query = _container.GetItemQueryIterator<T>(
            new QueryDefinition(queryText)
        );

        var results = new List<T>();

        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            results.AddRange(response);
        }

        return results;
    }

    public async Task<List<Hanzi>> GetRandomHanziList(int count)
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