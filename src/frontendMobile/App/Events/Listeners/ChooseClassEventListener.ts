import { GameRoomEventListener } from "..";
import { GameRoomData } from "../../Context/gameRoom";

type ChooseClassEventListenerKeyType = "Application.UseCases.GameRooms.GameActions.ChooseClassGameActionRequestResponse, Application";
export const ChooseClassEventListenerKey: ChooseClassEventListenerKeyType = "Application.UseCases.GameRooms.GameActions.ChooseClassGameActionRequestResponse, Application";

export interface ChooseClassEventListenerContent {
    Name: string;
    GameClassId: number;
    GameClassName: string;
    $type: ChooseClassEventListenerKeyType
}

export class ChooseClassEventListener extends GameRoomEventListener<ChooseClassEventListenerContent> {
    constructor(setRoom: React.Dispatch<React.SetStateAction<GameRoomData | null>>) {
        const onTrigger = (eventContet: ChooseClassEventListenerContent) => {
            setRoom(roomDispatch => {
                const playerTargetDict = roomDispatch!.players.map((anyPlayer, index) => {
                    return {
                        isTarget: anyPlayer.name == eventContet.Name,
                        player: anyPlayer,
                        index: index,
                    };
                });

                const targetPlayer = playerTargetDict.filter(x => x.isTarget)[0];

                targetPlayer.player.gameClass = {
                    name: eventContet.GameClassName,
                    id: eventContet.GameClassId,
                };

                return {
                    ...roomDispatch!,
                    players: [
                        ...roomDispatch!.players.filter(x => x != targetPlayer.player),
                        targetPlayer.player,
                    ]
                }
            });
        }

        super(ChooseClassEventListenerKey, onTrigger);
    }
}

