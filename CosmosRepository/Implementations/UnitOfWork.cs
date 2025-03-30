using CosmosRepository.Clients;
using CosmosRepository.Contracts;
using CosmosRepository.Entities.ElectricCalculator;
using CosmosRepository.Entities.HanziCollector;
using CosmosRepository.Entities.Users;
using CosmosRepository.Entities.Vocabulary;

namespace CosmosRepository.Implementations;

public class UnitOfWork(CosmosDbContext context) : IUnitOfWork
{
    private IRepository<Hanzi, string> _hanziRepository = new Repository<Hanzi, string>(context, "Hanzi", "/id");
    private IRepository<ElectricPrice, string> _electricPriceRepository = new Repository<ElectricPrice, string>(context, "ElectricPrice", "/Id");
    private IRepository<User, string> _userRepository = new Repository<User, string>(context, "User", "/Username");
    private IRepository<Vocabulary, string> _vocabularyRepository = new Repository<Vocabulary, string>(context, "Vocabulary", "/Id");

    public IRepository<Hanzi, string> Hanzis => _hanziRepository;
    public IRepository<ElectricPrice, string> ElectricPrices => _electricPriceRepository;
    public IRepository<User, string> Users => _userRepository;
    public IRepository<Vocabulary, string> Vocabularies => _vocabularyRepository;

    public Task SaveChangesAsync()
    {
        // No transactional unit of work in CosmosDB unless using transactional batch in same partition
        return Task.CompletedTask;
    }
}
