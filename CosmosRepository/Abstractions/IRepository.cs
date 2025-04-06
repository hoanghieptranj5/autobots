using System.Linq.Expressions;
using CosmosRepository.Entities;
using Microsoft.Azure.Cosmos;

namespace CosmosRepository.Abstractions;

public interface IRepository<T> where T : CosmosEntity
{
    Task<IEnumerable<T>> All();
    Task<T?> GetById(string id);
    Task<bool> Add(T entity);
    Task<bool> Delete(T entity);
    Task<bool> Delete(string id, PartitionKey partitionKey);
    Task<bool> Upsert(T entity);
    Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
}