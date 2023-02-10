namespace Repositories.UnitOfWork.Abstractions;

public interface IUnitOfWork
{
    ICarRepository Cars { get; set; }
    
    Task CompleteAsync();
}