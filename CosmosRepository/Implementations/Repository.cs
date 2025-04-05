using System.Linq.Expressions;
using CosmosRepository.Abstractions;
using CosmosRepository.Clients;
using CosmosRepository.Entities;
using CosmosRepository.Entities.HanziCollector;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace CosmosRepository.Implementations;

public class Repository<T, R> : IRepository<T, R> where T : BaseEntity
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
}