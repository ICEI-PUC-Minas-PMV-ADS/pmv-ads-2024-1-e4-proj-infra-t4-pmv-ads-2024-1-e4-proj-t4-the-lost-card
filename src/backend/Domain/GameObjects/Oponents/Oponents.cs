using Domain.Entities;
using Domain.GameObjects.Oponents.Tester;

namespace Domain.GameObjects.Oponents;

public class Oponents
{
    public static IEnumerable<Oponent> All { get; } = new Oponent[] {
        new TesterOponent()
    };

    public static Dictionary<long, Oponent> Dictionary { get; } = All.ToDictionary(gc => gc.Id);
}

public abstract class Oponent : GameObjectBaseClass
{
    public override string QueryKey { get; } = typeof(Oponent).FullName!;

    public abstract int StartingMaxLife { get; }
    public abstract int MinLevel { get; }
    public abstract int MaxLevel { get; }
    public abstract OponentIntent GetIntent(GameRoom gameRoom);
}
