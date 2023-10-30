using Microsoft.Extensions.Logging;
using Repositories.Models;
using Repositories.Models.Users;
using Repositories.UnitOfWork.Abstractions;

namespace Repositories.UnitOfWork;

public class UserRepository : Repository<User, int>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext, ILogger logger) : base(dbContext, logger)
    {
    }
}
