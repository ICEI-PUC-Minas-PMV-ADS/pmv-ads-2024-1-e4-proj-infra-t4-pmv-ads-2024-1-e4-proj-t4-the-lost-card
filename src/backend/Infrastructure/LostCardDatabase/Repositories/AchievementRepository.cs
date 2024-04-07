using Application.Contracts.LostCardDatabase;
using Domain.Entities;

namespace Infrastructure.LostCardDatabase.Repositories;

public class AchievementRepository : IAchievementRepository
{
    private readonly LostCardDbContext lostCardsContext;
    public AchievementRepository(LostCardDbContext context)
    {
        this.lostCardsContext = context;
    }

    public async Task Create(Achievements achievement, CancellationToken cancellationToken = default)
    {
        await lostCardsContext.Achievements.AddAsync(achievement, cancellationToken);
    }

    public void Delete(Guid id)
    {
        lostCardsContext.Remove(id);
    }

    public void AddPlayerNewAchievement(Player player)
    {
        lostCardsContext.Players.Update(player);
    }
}
