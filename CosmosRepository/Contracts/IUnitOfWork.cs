using CosmosRepository.Entities.ElectricCalculator;
using CosmosRepository.Entities.HanziCollector;
using CosmosRepository.Entities.Users;
using CosmosRepository.Entities.Vocabulary;

namespace CosmosRepository.Contracts;

public interface IUnitOfWork
{
    IRepository<Hanzi, string> Hanzis { get; }
    IRepository<ElectricPrice, string> ElectricPrices { get; }
    IRepository<User, string> Users { get; }
    IRepository<Vocabulary, string> Vocabularies { get; }
    Task SaveChangesAsync(); // Placeholder, CosmosDB doesn't support transactions easily
}