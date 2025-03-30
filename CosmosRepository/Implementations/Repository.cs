using System.Linq.Expressions;
using CosmosRepository.Clients;
using CosmosRepository.Contracts;
using CosmosRepository.Entities;
using Microsoft.Azure.Cosmos;

namespace CosmosRepository.Implementations;

public class Repository<T, R> : IRepository<T, R> where T : BaseEntity
{
    private readonly Container _container;

    public Repository(CosmosDbContext cosmosDbContext, string containerName)
    {
        _container = cosmosDbContext.GetContainer(containerName);
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
        // Fetch all and filter in memory (not ideal for big datasets)
        var allItems = await All();
        return allItems.AsQueryable().Where(predicate);
    }
}