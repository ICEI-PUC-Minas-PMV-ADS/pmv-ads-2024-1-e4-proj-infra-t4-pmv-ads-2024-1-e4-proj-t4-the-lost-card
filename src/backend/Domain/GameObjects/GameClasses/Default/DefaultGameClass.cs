namespace Domain.GameObjects.GameClasses.Default;

public class DefaultGameClass
{
    public static GameClass Value { get; } = new GameClass(
        IdAssignHelper.CalculateIdHash(nameof(DefaultGameClass)), 
        "Default",
        new Card[] {
            new BlockCard()
        },
        new Card[] {
            new BlockCard(),
            new BlockCard(),
            new BlockCard()
        },
        100
    );
}
