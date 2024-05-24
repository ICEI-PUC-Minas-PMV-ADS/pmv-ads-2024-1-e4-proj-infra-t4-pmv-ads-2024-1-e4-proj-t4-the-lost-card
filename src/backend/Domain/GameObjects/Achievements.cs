namespace Domain.GameObjects;

public class Achievements
{
    public static IEnumerable<Achievment> All { get; } = new Achievment[] {
        new(IdAssignHelper.CalculateIdHash("Achiev: Join a game"), "Join a game", "Join a game room for the first time"),
        new(IdAssignHelper.CalculateIdHash("Achiev: Finish a game"), "Finish a game", "Reach the end of game, winning or losing")
    };

    public static Dictionary<long, Achievment> Dictionary { get; } = All.ToDictionary(a => a.Id);

    public record Achievment(
        long Id,
        string Title,
        string Description,
        string IconPath = "defaultpath" //TODO: Criar icones das conquistas
    ) : GameObjectBaseRecord(Id);
}
