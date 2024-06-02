import { GameRoomEventDispatch, Typed } from ".."

type JoinGameRoomEventDispatchType = "Application.UseCases.GameRooms.LobbyActions.JoinGameRoomHubRequest, Application";
const JoinGameRoomEventDispatchKey: JoinGameRoomEventDispatchType = "Application.UseCases.GameRooms.LobbyActions.JoinGameRoomHubRequest, Application";

export interface JoinGameRoomEventDispatchContent extends Typed {
    $type: JoinGameRoomEventDispatchType;
    roomGuid: string | null;
    creationOptions: { RoomName: string } | null
}

export class JoinGameRoomEventDispatch implements GameRoomEventDispatch<JoinGameRoomEventDispatchContent> {
    constructor(roomGuid: string | null, creationOptions: { RoomName: string } | null) {
        this.dispatchKey = JoinGameRoomEventDispatchKey;
        this.dispatchFactory = () => {
            return {
                $type: JoinGameRoomEventDispatchKey,
                roomGuid: roomGuid,
                creationOptions: creationOptions
            }
        }
    }

    dispatchKey: string;
    dispatchFactory: () => JoinGameRoomEventDispatchContent;
}