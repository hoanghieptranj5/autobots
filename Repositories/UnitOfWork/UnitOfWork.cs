using Microsoft.Extensions.Logging;
using Repositories.Models;
using Repositories.UnitOfWork.Abstractions;

namespace Repositories.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly ApplicationDbContext _dbContext;

    public ICarRepository Cars { get; set; }
    public IElectricPriceRepository ElectricPrices { get; set; }

    public UnitOfWork(ApplicationDbContext dbContext, ILoggerFactory loggerFactory)
    {
        _dbContext = dbContext;
        var logger = loggerFactory.CreateLogger<UnitOfWork>();
        
        Cars = new CarRepository(dbContext, logger);
    }

    public async Task CompleteAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}