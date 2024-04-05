namespace Application.Contracts.LostCardDatabase;

public interface ILostCardDbUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
