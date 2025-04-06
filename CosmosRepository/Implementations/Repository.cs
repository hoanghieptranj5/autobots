using System.Linq.Expressions;
using CosmosRepository.Abstractions;
using CosmosRepository.Clients;
using CosmosRepository.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace CosmosRepository.Implementations;

public class Repository<T> : IRepository<T> where T : CosmosEntity
{
    protected readonly Container _container;

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

    public Task<T?> GetById(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Add(T entity)
    {
        try
        {
            await _container.CreateItemAsync(entity, entity.GetPartitionKey());
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> Delete(T entity)
    {
        try
        {
            await _container.DeleteItemAsync<T>(entity.Id, entity.GetPartitionKey());
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> Delete(string id, PartitionKey partitionKey)
    {
        try
        {
            await _container.DeleteItemAsync<T>(id, partitionKey);
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
            await _container.UpsertItemAsync(entity, entity.GetPartitionKey());
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// If your LINQ query includes a filter on the partition key field,
    /// the SDK can automatically infer the partition key from the query itself,
    /// eliminating the need to specify it separately. Otherwise, it executes cross-partition query.
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
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
}