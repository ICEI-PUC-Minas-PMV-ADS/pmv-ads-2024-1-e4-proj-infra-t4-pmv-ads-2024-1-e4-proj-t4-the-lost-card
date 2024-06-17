import { GameRoomEventListener } from "..";
import { GameRoomData, GameRoomPlayerData } from "../../Context/gameRoom";

type JoinGameRoomEventListeningKeyType = "Application.UseCases.GameRooms.LobbyActions.JoinGameRoomHubRequestResponse, Application";
export const JoinGameRoomEventListeningKey: JoinGameRoomEventListeningKeyType = "Application.UseCases.GameRooms.LobbyActions.JoinGameRoomHubRequestResponse, Application";

export interface JoinGameRoomEventListenerContent {
    RoomId: string;
    AdminName: string;
    $type: JoinGameRoomEventListeningKeyType;
    Players: { Name: string; Class: { Name: string; Id: number } | null }[];
}

export class JoinGameRoomEventListener extends GameRoomEventListener<JoinGameRoomEventListenerContent> {
    constructor(setRoom: React.Dispatch<React.SetStateAction<GameRoomData | null>>, currentUserName: string) {
        const onTrigger = (eventContet: JoinGameRoomEventListenerContent) => {
            setRoom(roomDispatch => {
                return {
                    ...(roomDispatch ?? {}),
                    id: eventContet.RoomId,
                    adminName: eventContet.AdminName,
                    hasStarted: false,
                    oponnent: null,
                    currentLevel: 0,
                    currentTurn: 0,
                    players: eventContet.Players.map(x => new GameRoomPlayerData(
                        x.Name,
                        x.Name == currentUserName,
                        x.Class ? { name: x.Class.Name, id: x.Class.Id } : null
                    ))
                };
            });
        }

        super(JoinGameRoomEventListeningKey, onTrigger);
    }
}

