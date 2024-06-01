using Domain.Entities;
using Domain.GameObjects.Oponents.Intents;

namespace Domain.GameObjects.Oponents.Tester;

public class TesterOponent : Oponent
{
    public override long Id => IdAssignHelper.CalculateIdHash(nameof(Tester));
    public override int MaxLife { get; set; } = 100;
    public override int MinLevel => 1;
    public override int MaxLevel => 5;

    public override OponentIntent GetNewIntent(GameRoom gameRoom)
    {
        return new AttackIntent() { DamageAmount = 10 };
    }
}
