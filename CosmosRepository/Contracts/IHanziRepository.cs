using CosmosRepository.Entities;
using CosmosRepository.Entities.HanziCollector;

namespace CosmosRepository.Contracts;

public interface IHanziRepository<T, R> : IRepository<T, R> where T : BaseEntity
{
    Task<List<Hanzi>> GetRandomHanziList(int count);
}