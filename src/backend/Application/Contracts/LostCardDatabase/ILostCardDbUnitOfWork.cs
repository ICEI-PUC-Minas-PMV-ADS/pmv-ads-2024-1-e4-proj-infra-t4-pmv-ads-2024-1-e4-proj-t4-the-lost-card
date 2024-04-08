namespace Application.Contracts.LostCardDatabase;

public interface ILostCardDbUnitOfWork
{
    IPlayerRepository PlayerRepository { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
