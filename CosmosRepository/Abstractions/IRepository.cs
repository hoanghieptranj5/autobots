using System.Linq.Expressions;
using CosmosRepository.Entities;

namespace CosmosRepository.Abstractions;

public interface IRepository<T, R> where T : BaseEntity
{
    Task<IEnumerable<T>> All();
    Task<T?> GetById(R id);
    Task<bool> Add(T entity);
    Task<bool> Delete(R id);
    Task<bool> Upsert(T entity);
    Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
}