﻿using Domain.Entities;
using Domain.GameObjects.GameClasses.Default;

namespace Domain.GameObjects;

public class Card : GameObjectBaseClass
{
    public override string QueryKey => typeof(Card).FullName!;

    public virtual int? GameClassId { get; } = null;

    public virtual string Name { get; } = "Carta padrão";

    public virtual string Description { get; } = "Não faz absolutamente nada";

    public virtual void OnPlay(GameRoom gameRoom, GameRoom.RoomGameInfo.PlayerGameInfo playerGameInfo) { }
}

public class Cards
{
    public static IEnumerable<Card> All { get; } = 
        Enumerable.Empty<Card>()
            .Concat(DefaultGameClass.Value.AvailableCards);
}