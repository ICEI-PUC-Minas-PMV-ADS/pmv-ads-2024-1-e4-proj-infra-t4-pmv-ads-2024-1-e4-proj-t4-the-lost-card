using Domain.Entities;

namespace Application.Contracts.LostCardDatabase;

public interface IAchievementRepository
{
    Task Create(Achievements achievement, CancellationToken cancellationToken = default);
    void AddPlayerNewAchievement(Player player);
    void Delete(Guid id);
}
