import { GameRoomEventListener } from "..";
import { GameRoomData } from "../../Context/gameRoom";

type TurnStartedEventListenerKeyType = "Application.UseCases.GameRooms.GameEvents.TurnStartedNotifcationDispatch, Application";
export const TurnStartedEventListenerKey: TurnStartedEventListenerKeyType = "Application.UseCases.GameRooms.GameEvents.TurnStartedNotifcationDispatch, Application";

export interface TurnStartedEventListenerContent {
    IsNewLevel: boolean,
    $type: TurnStartedEventListenerKeyType
}

export class TurnStartedEventListener extends GameRoomEventListener<TurnStartedEventListenerContent> {
    constructor(setRoom: React.Dispatch<React.SetStateAction<GameRoomData | null>>) {
        const onTrigger = (eventContet: TurnStartedEventListenerContent) => {
            setRoom(roomDispatch => {
                return {
                    ...roomDispatch!,
                    currentLevel: eventContet.IsNewLevel ? roomDispatch!.currentLevel + 1 : roomDispatch!.currentLevel,
                    currentTurn: eventContet.IsNewLevel ? 0 : roomDispatch!.currentTurn + 1,
                    players: roomDispatch!.players.map(player => {
                        return {
                            ...player,
                            ActionsFinished: false
                        }
                    })
                }
            });
        }

        super(TurnStartedEventListenerKey, onTrigger);
    }
}

