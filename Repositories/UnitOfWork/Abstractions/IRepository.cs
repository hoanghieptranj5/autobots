using System.Linq.Expressions;

namespace Repositories.UnitOfWork.Abstractions;

public interface IRepository<T, R> where T : class
{
    Task<IEnumerable<T>> All();
    IQueryable<T> AllQuery();
    Task<T?> GetById(R id);
    Task<bool> Add(T entity);
    Task<bool> Delete(R id);
    Task<bool> Upsert(T entity);
    Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
}