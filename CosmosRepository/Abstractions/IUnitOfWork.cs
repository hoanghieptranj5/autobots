using CosmosRepository.Entities.ElectricCalculator;
using CosmosRepository.Entities.HanziCollector;
using CosmosRepository.Entities.Users;
using CosmosRepository.Entities.Vocabulary;

namespace CosmosRepository.Abstractions;

public interface IUnitOfWork
{
    IHanziRepository<Hanzi> Hanzis { get; }
    IRepository<ElectricPrice> ElectricPrices { get; }
    IRepository<User> Users { get; }
    IRepository<Vocabulary> Vocabularies { get; }
}