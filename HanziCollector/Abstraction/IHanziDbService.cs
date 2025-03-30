using System.Collections;
using CosmosRepository.Entities.HanziCollector;

namespace HanziCollector.Abstraction;

internal interface IHanziDbService
{
    Task<bool> SaveSingle(Hanzi hanzi);
    Task<IEnumerable<Hanzi>> ReadAll();
    Task<IEnumerable<Hanzi>> ReadRange(int skip, int take);
    Task<IEnumerable<Hanzi>> ReadRandomHanziList();
    Task<bool> DeleteSingle(string id);
    Task<bool> UpdateSingle(Hanzi hanzi);
}
