namespace Domain.GameObjects.GameClasses.Default;

public class DefaultGameClass
{
    public static GameClass Value { get; } = new GameClass(
        IdAssignHelper.CalculateIdHash(nameof(DefaultGameClass)), 
        "Default",
        new Card[] {
            new BlockCard(),
            new StrikeCard()
        },
        new Card[] {
            new BlockCard(),
            new BlockCard(),
            new BlockCard(),
            new BlockCard(),
            new BlockCard(),
            new StrikeCard(),
            new StrikeCard(),
            new StrikeCard(),
            new StrikeCard(),
            new StrikeCard(),
        },
        100,
        3
    );
}
