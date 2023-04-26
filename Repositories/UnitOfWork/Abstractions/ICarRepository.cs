using Repositories.Models;

namespace Repositories.UnitOfWork.Abstractions;

public interface ICarRepository : IRepository<Car, int>
{
}