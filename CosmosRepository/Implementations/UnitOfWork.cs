using CosmosRepository.Abstractions;
using CosmosRepository.Clients;
using CosmosRepository.Entities.ElectricCalculator;
using CosmosRepository.Entities.HanziCollector;
using CosmosRepository.Entities.Users;
using CosmosRepository.Entities.Vocabulary;

namespace CosmosRepository.Implementations;

public class UnitOfWork(CosmosDbContext context) : IUnitOfWork
{
    private IHanziRepository<Hanzi> _hanziRepository = new HanziRepository(context, "Hanzis", "/Bucket");
    private IRepository<ElectricPrice> _electricPriceRepository = new Repository<ElectricPrice>(context, "ElectricPrice", "/Id");
    private IRepository<User> _userRepository = new Repository<User>(context, "User", "/Username");
    private IRepository<Vocabulary> _vocabularyRepository = new Repository<Vocabulary>(context, "Vocabulary", "/Id");

    public IHanziRepository<Hanzi> Hanzis => _hanziRepository;
    public IRepository<ElectricPrice> ElectricPrices => _electricPriceRepository;
    public IRepository<User> Users => _userRepository;
    public IRepository<Vocabulary> Vocabularies => _vocabularyRepository;
}
