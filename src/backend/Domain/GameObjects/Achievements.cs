namespace Domain.GameObjects;

public class Achievements
{
    public static IEnumerable<Achievment> All { get; } = new Achievment[] {
        new(GlobalCounter.Instance++, "Join a game", "Join a game room for the first time"),
        new(GlobalCounter.Instance++, "Finish a game", "Reach the end of game, winning or losing"),
        new(GlobalCounter.Instance++, "Finish a game", "Reach the end of game, winning or losing")
    };

    public static Dictionary<int, Achievment> Dictionary { get; } = All.ToDictionary(a => a.Id);

    public record Achievment(
        int Id,
        string Title,
        string Description,
        string IconPath = "defaultpath" //TODO: Criar icones das conquistas
    ) : GameObjectBaseRecord(Id);
}
