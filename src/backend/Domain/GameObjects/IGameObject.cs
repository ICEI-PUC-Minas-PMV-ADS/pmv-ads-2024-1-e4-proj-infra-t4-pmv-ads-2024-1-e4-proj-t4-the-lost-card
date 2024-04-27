namespace Domain.GameObjects;

public class GameObjects
{
    public static IEnumerable<IGameObject> All { get; } =
        Enumerable.Empty<IGameObject>()
            .Concat(Achievements.All)
            .Concat(GameClasses.GameClasses.All)
            .Concat(Cards.All);
}

public interface IGameObject
{
    int Id { get; }

    string QueryKey { get; }
}

public class GameObjectBaseClass : IGameObject
{
    public virtual int Id { get; }

    public virtual string QueryKey => GetType().FullName!;
}

public record GameObjectBaseRecord(int Id) : IGameObject
{
    public virtual string QueryKey => GetType().FullName!;
}
