using Repositories.Models.HanziCollector;

namespace Repositories.UnitOfWork.Abstractions;

public interface IHanziRepository : IRepository<Hanzi, string>
{
}
