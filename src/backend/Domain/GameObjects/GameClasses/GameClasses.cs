using Domain.GameObjects.GameClasses.Default;

namespace Domain.GameObjects.GameClasses;

public record GameClass(
    int Id, 
    string Name, 
    IEnumerable<Card> AvailableCards, 
    IEnumerable<Card> StartingHand,
    int StartingLife
) : GameObjectBaseRecord(Id);

public class GameClasses
{
    public static IEnumerable<GameClass> All { get; } = new GameClass[] {
        DefaultGameClass.Value,
    };

    public static Dictionary<int, GameClass> Dictionary { get; } = All.ToDictionary(gc => gc.Id);
}
