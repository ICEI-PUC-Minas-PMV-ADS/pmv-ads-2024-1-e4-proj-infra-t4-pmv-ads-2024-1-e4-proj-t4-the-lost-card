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
    long Id { get; }
    string QueryKey { get; }
}

public abstract class GameObjectBaseClass : IGameObject
{
    public abstract long Id { get; }
    public virtual string QueryKey { get; } = typeof(GameObjectBaseClass).FullName!;
}

public abstract record GameObjectBaseRecord(long Id) : IGameObject
{
    public virtual string QueryKey => typeof(GameObjectBaseRecord).FullName!;
}
