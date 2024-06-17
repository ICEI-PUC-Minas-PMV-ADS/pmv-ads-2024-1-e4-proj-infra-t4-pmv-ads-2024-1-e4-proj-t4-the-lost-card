import { GameRoomEventListener } from "..";
import { GameRoomData } from "../../Context/gameRoom";

type EndTurnEventListenerKeyType = "Application.UseCases.GameRooms.GameActions.EndTurnGameActionRequestResponse, Application";
export const EndTurnEventListenerKey: EndTurnEventListenerKeyType = "Application.UseCases.GameRooms.GameActions.EndTurnGameActionRequestResponse, Application";

export interface EndTurnEventListenerContent {
    PlayerName: string;
    $type: EndTurnEventListenerKeyType
}

export class EndTurnEventListener extends GameRoomEventListener<EndTurnEventListenerContent> {
    constructor(setRoom: React.Dispatch<React.SetStateAction<GameRoomData | null>>) {
        const onTrigger = (eventContet: EndTurnEventListenerContent) => {
            setRoom(roomDispatch => {
                const playerTargetDict = roomDispatch!.players.map((anyPlayer, index) => {
                    return {
                        isTarget: anyPlayer.name == eventContet.PlayerName,
                        player: anyPlayer,
                        index: index,
                    };
                });

                const targetPlayer = playerTargetDict.filter(x => x.isTarget)[0];

                return {
                    ...roomDispatch!,
                    players: [
                        ...roomDispatch!.players.filter(x => x != targetPlayer.player),
                        {
                            ...targetPlayer.player,
                            ActionsFinished: true
                        }
                    ]
                }
            });
        }

        super(EndTurnEventListenerKey, onTrigger);
    }
}

