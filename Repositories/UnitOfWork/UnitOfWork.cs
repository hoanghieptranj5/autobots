using Microsoft.Extensions.Logging;
using Repositories.Models;
using Repositories.UnitOfWork.Abstractions;

namespace Repositories.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly ApplicationDbContext _dbContext;

    public IElectricPriceRepository ElectricPrices { get; set; }
    public IHanziRepository Hanzis { get; set; }
    public IUserRepository Users { get; set; }
    public IVocabularyRepository Vocabularies { get; set; }

    public UnitOfWork(ApplicationDbContext dbContext, ILoggerFactory loggerFactory)
    {
        _dbContext = dbContext;
        var logger = loggerFactory.CreateLogger<UnitOfWork>();

        ElectricPrices = new ElectricPriceRepository(dbContext, logger);
        Hanzis = new HanziRepository(dbContext, logger);
        Users = new UserRepository(dbContext, logger);
        Vocabularies = new VocabularyRepository(dbContext, logger);
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
