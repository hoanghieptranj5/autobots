using Microsoft.Extensions.Logging;
using Repositories.Models;
using Repositories.Models.ElectricCalculator;
using Repositories.UnitOfWork.Abstractions;

namespace Repositories.UnitOfWork;

public class ElectricPriceRepository : Repository<ElectricPrice>, IElectricPriceRepository
{
    public ElectricPriceRepository(ApplicationDbContext dbContext, ILogger logger) : base(dbContext, logger)
    {
    }
}