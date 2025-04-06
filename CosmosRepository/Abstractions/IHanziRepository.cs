using CosmosRepository.Entities;
using CosmosRepository.Entities.HanziCollector;

namespace CosmosRepository.Abstractions;

public interface IHanziRepository<T> : IRepository<T> where T : CosmosEntity
{
    Task<List<Hanzi>> GetRandomHanziList(int count);
}