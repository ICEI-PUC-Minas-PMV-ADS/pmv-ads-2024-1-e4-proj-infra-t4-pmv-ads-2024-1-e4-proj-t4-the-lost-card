import { GameRoomEventListener, Typed } from "..";
import { GameRoomData } from "../../Context/gameRoom";

type PlayCardEventListenerKeyType = "Application.UseCases.GameRooms.GameActions.PlayCardGameActionRequestResponse, Application";
const PlayCardEventListenerKey: PlayCardEventListenerKeyType = "Application.UseCases.GameRooms.GameActions.PlayCardGameActionRequestResponse, Application";

export interface PlayCardEventListenerContent extends Typed {
    $type: PlayCardEventListenerKeyType;
    PlayerName: string;
    CardName: string;
    CardId: number;
}

export class PlayCardEventListener extends GameRoomEventListener<PlayCardEventListenerContent> {
    constructor(setRoom: React.Dispatch<React.SetStateAction<GameRoomData | null>>) {
        const onTrigger = (eventContet: PlayCardEventListenerContent) => {
            setRoom(roomDispatch => {
                const playerTargetDict = roomDispatch!.players.map((anyPlayer, index) => {
                    return {
                        isTarget: anyPlayer.name == eventContet.PlayerName,
                        player: anyPlayer,
                        index: index,
                    };
                });
                
                const targetedPlayer = playerTargetDict.filter(x => x.isTarget)[0];

                const cardTargetDict = targetedPlayer.player.Hand.map((anyCard, index) => {
                    return {
                        isTarget: anyCard.Id == eventContet.CardId,
                        card: anyCard,
                        index: index,
                    };
                });

                const targetedCard = cardTargetDict.filter(x => x.isTarget)[0];

                return {
                    ...roomDispatch!,
                    players: [
                        ...roomDispatch!.players.filter((_, index) => index != targetedPlayer.index),
                        {
                            ...targetedPlayer.player,
                            ...(targetedPlayer.player.isMe ? {
                                Hand: [...targetedPlayer.player.Hand.filter((_, index) => index != targetedCard.index)],
                                DiscardPile: [...targetedPlayer.player.DiscardPile, targetedCard.card]
                            } : {})
                        }
                    ]
                };
            });
        }

        super(PlayCardEventListenerKey, onTrigger);
    }
}