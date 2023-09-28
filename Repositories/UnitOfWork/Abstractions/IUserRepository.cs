using Repositories.Models.Users;

namespace Repositories.UnitOfWork.Abstractions;

public interface IUserRepository : IRepository<User, int>
{
  
}