using Domain.GameObjects.GameClasses.Default;

namespace Domain.GameObjects.GameClasses;

public record GameClass(
    long Id,
    string Name,
    IEnumerable<Card> AvailableCards,
    IEnumerable<Card> StartingHand,
    int StartingLife
) : GameObjectBaseRecord(Id)
{
    public override string QueryKey => typeof(GameClass).FullName!;
};

public class GameClasses
{
    public static IEnumerable<GameClass> All { get; } = new GameClass[] {
        DefaultGameClass.Value,
    };

    public static Dictionary<long, GameClass> Dictionary { get; } = All.ToDictionary(gc => gc.Id);
}
