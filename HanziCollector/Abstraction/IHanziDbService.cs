using CosmosRepository.Entities.HanziCollector;

namespace HanziCollector.Abstraction;

internal interface IHanziDbService
{
    Task<bool> SaveSingle(Hanzi hanzi);
    Task<IEnumerable<Hanzi>> ReadAll();
    Task<IEnumerable<Hanzi>> ReadRandomHanziList(int length);
    Task<bool> DeleteSingle(string id);
    Task<bool> UpdateSingle(Hanzi hanzi);
}
