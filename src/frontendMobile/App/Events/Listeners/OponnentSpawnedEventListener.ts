import { GameRoomEventListener, Typed } from "..";
import { GameRoomData } from "../../Context/gameRoom";

type OponnentSpawnedEventListenerKeyType = "Application.UseCases.GameRooms.GameEvents.OponentSpawnedNotificationDispatch, Application";
const OponnentSpawnedEventListenerKey: OponnentSpawnedEventListenerKeyType = "Application.UseCases.GameRooms.GameEvents.OponentSpawnedNotificationDispatch, Application";

export interface OponnentSpawnedEventListenerContent extends Typed {
    $type: OponnentSpawnedEventListenerKeyType;
    GameId: number;
    MaxLife: number;
    CurrentLife: number;
    Intent: { $type: string, Id: number, Type: number, DamageAmount: number | undefined };
}

export class OponnentSpawnedEventListener extends GameRoomEventListener<OponnentSpawnedEventListenerContent> {
    constructor(setRoom: React.Dispatch<React.SetStateAction<GameRoomData | null>>) {
        const onTrigger = (eventContet: OponnentSpawnedEventListenerContent) => {
            setRoom(roomDispatch => {
                return {
                    ...roomDispatch!,
                    currentLevel: roomDispatch!.currentLevel + 1,
                    currentTurn: 0,
                    oponnent: {
                        CurrentLife: eventContet.CurrentLife,
                        CurrentBlock: 0,
                        id: eventContet.GameId,
                        MaxLife: eventContet.MaxLife,
                        intent: { $type: eventContet.Intent.$type, id: eventContet.Intent.Id, type: eventContet.Intent.Type, damageAmount: eventContet.Intent.DamageAmount }
                    }
                };
            });
        }

        super(OponnentSpawnedEventListenerKey, onTrigger);
    }
}