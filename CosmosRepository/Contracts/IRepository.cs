using System.Linq.Expressions;
using CosmosRepository.Entities;
using CosmosRepository.Entities.HanziCollector;

namespace CosmosRepository.Contracts;

public interface IRepository<T, R> where T : BaseEntity
{
    Task<IEnumerable<T>> All();
    IQueryable<T> AllQuery();
    Task<T?> GetById(R id);
    Task<bool> Add(T entity);
    Task<bool> Delete(R id);
    Task<bool> Upsert(T entity);
    Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> SelectIn(List<int> secondaryFieldIds);
    Task<List<Hanzi>> GetRandomHanziList(int count);
}