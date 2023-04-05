using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repositories.Models;
using Repositories.UnitOfWork.Abstractions;

namespace Repositories.UnitOfWork;

public class Repository<T, R> : IRepository<T, R> where T : class
{
    private readonly DbSet<T> _dbSet;
    private readonly ILogger _logger;

    protected Repository(ApplicationDbContext dbContext, ILogger logger)
    {
        _dbSet = dbContext.Set<T>();
        _logger = logger;
    }

    public async Task<IEnumerable<T>> All()
    {
        return await _dbSet.ToListAsync();
    }

    public IQueryable<T> AllQuery()
    {
        return _dbSet.AsQueryable();
    }

    public async Task<T?> GetById(R id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<bool> Add(T entity)
    {
        try
        {
            await _dbSet.AddAsync(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return false;
        }

        return true;
    }

    public async Task<bool> Delete(R id)
    {
        var entity = await GetById(id);
        if (entity == null)
        {
            _logger.LogDebug($"{id} not found");
            return false;
        }

        _dbSet.Remove(entity);
        return true;
    }

    public Task<bool> Upsert(T entity)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }
}