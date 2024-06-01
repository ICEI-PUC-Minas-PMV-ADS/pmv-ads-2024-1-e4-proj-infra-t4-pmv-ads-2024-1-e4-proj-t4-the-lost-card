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
    public abstract int MaxLife { get; set; }
    public int CurrentLife { get; set; }
    public int CurrentBlock { get; set; }
    public abstract int MinLevel { get; }
    public abstract int MaxLevel { get; }

    public OponentIntent? CurrentIntent { get; set; }
    public abstract OponentIntent GetNewIntent(GameRoom gameRoom);
}
