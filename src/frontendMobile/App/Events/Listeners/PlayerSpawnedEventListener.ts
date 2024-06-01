import { GameRoomEventListener, Typed } from "..";
import { GameRoomData } from "../../Context/gameRoom";

type PlayerSpawnedEventListenerKeyType = "Application.UseCases.GameRooms.GameEvents.PlayerSpawnedNotificationDispatch, Application";
const PlayerSpawnedEventListenerKey: PlayerSpawnedEventListenerKeyType = "Application.UseCases.GameRooms.GameEvents.PlayerSpawnedNotificationDispatch, Application";

export interface PlayerSpawnedEventListenerContent extends Typed {
    $type: PlayerSpawnedEventListenerKeyType;
    Name: string;
    MaxLife: number;
    CurrentLife: number;
    CurrentBlock: number;
    GameClassId: number;
    GameClassName: string;
    CurrentEnergy: number;
    MaxEnergy: number;
}

export class PlayerSpawnedEventListener extends GameRoomEventListener<PlayerSpawnedEventListenerContent> {
    constructor(setRoom: React.Dispatch<React.SetStateAction<GameRoomData | null>>) {
        const onTrigger = (eventContet: PlayerSpawnedEventListenerContent) => {
            setRoom(roomDispatch => {
                const playerTargetDict = roomDispatch!.players.map((anyPlayer, index) => {
                    return {
                        isTarget: anyPlayer.name == eventContet.Name,
                        player: anyPlayer,
                        index: index,
                    };
                });

                const targetedPlayer = playerTargetDict.filter(x => x.isTarget)[0];

                targetedPlayer.player = {
                    ...targetedPlayer.player,
                    CurrentLife: eventContet.CurrentLife,
                    MaxLife: eventContet.MaxLife,
                    CurrentBlock: eventContet.CurrentBlock,
                    CurrentEnergy: eventContet.CurrentEnergy,
                    MaxEnergy: eventContet.MaxEnergy,
                    gameClass: { id: eventContet.GameClassId, name: eventContet.Name }
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

        super(PlayerSpawnedEventListenerKey, onTrigger);
    }
}