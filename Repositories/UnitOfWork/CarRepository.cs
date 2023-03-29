using Microsoft.Extensions.Logging;
using Repositories.Models;
using Repositories.UnitOfWork.Abstractions;

namespace Repositories.UnitOfWork;

public class CarRepository : Repository<Car, int>, ICarRepository
{
    public CarRepository(ApplicationDbContext dbContext, ILogger logger) : base(dbContext, logger)
    {
        
    }
}