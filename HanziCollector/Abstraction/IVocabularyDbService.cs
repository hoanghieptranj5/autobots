
using CosmosRepository.Entities.Vocabulary;

namespace HanziCollector.Abstraction;

public interface IVocabularyDbService
{
    Task<bool> SaveSingle(Vocabulary vocabulary);
    Task<IEnumerable<Vocabulary>> ReadAll();
    Task<IEnumerable<Vocabulary>> ReadRange(int skip, int take);
    Task<IEnumerable<Vocabulary>> ReadRandomList();
    Task<bool> DeleteSingle(string id);
    Task<bool> UpdateSingle(Vocabulary vocabulary);
}
