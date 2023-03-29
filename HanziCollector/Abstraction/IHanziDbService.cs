using Repositories.Models.HanziCollector;

namespace HanziCollector.Abstraction;

public interface IHanziDbService
{ 
    Task<bool> SaveSingle(Hanzi hanzi);
    Task<IEnumerable<Hanzi>> ReadAll();
    Task<bool> DeleteSingle(string id);
    Task<bool> UpdateSingle(Hanzi hanzi);
}