using Microsoft.Extensions.Logging;
using Repositories.Models;
using Repositories.Models.Vocabulary;
using Repositories.UnitOfWork.Abstractions;

namespace Repositories.UnitOfWork;

public class VocabularyRepository : Repository<Vocabulary, int>, IVocabularyRepository
{
    public VocabularyRepository(ApplicationDbContext dbContext, ILogger logger) : base(dbContext, logger)
    {
    }
}
