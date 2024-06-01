import { GameRoomEventListener, Typed } from "..";
import { Card, GameRoomData } from "../../Context/gameRoom";

type HandShuffledEventListenerKeyType = "Application.UseCases.GameRooms.GameEvents.HandShuffledNotificationDispatch, Application";
const HandShuffledEventListenerKey: HandShuffledEventListenerKeyType = "Application.UseCases.GameRooms.GameEvents.HandShuffledNotificationDispatch, Application";

export interface HandShuffledEventListenerContent extends Typed {
    $type: HandShuffledEventListenerKeyType;
    PlayerName: string;
    Hand: Card[];
    DrawPile: Card[];
    DiscardPile: Card[];
}

export class HandShuffledEventListener extends GameRoomEventListener<HandShuffledEventListenerContent> {
    constructor(setRoom: React.Dispatch<React.SetStateAction<GameRoomData | null>>) {
        const onTrigger = (eventContet: HandShuffledEventListenerContent) => {
            setRoom(roomDispatch => {
                const playerTargetDict = roomDispatch!.players.map((anyPlayer, index) => {
                    return {
                        isTarget: anyPlayer.name == eventContet.PlayerName,
                        player: anyPlayer,
                        index: index,
                    };
                });

                const targetedPlayer = playerTargetDict.filter(x => x.isTarget)[0];

                targetedPlayer.player = {
                    ...targetedPlayer.player,
                    Hand: eventContet.Hand,
                    DrawPile: eventContet.DrawPile,
                    DiscardPile: eventContet.DiscardPile
                }

                return {
                    ...roomDispatch!,
                    players: [
                        ...roomDispatch!.players.filter((_, index) => index != targetedPlayer.index),
                        targetedPlayer.player
                    ]
                };
            });
        }

        super(HandShuffledEventListenerKey, onTrigger);
    }
}