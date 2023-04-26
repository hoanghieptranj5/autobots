using Microsoft.Extensions.Logging;
using Repositories.Models;
using Repositories.Models.HanziCollector;
using Repositories.UnitOfWork.Abstractions;

namespace Repositories.UnitOfWork;

public class HanziRepository : Repository<Hanzi, string>, IHanziRepository
{
  public HanziRepository(ApplicationDbContext dbContext, ILogger logger) : base(dbContext, logger)
  {
  }
}