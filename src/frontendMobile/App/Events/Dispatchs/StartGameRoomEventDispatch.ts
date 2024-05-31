import { GameRoomEventDispatch, Typed } from ".."

type StartGameRoomEventDispatchType = "Application.UseCases.GameRooms.LobbyActions.StartGameRoomHubRequest, Application";
const StartGameRoomEventDispatchKey: StartGameRoomEventDispatchType = "Application.UseCases.GameRooms.LobbyActions.StartGameRoomHubRequest, Application";

export interface StartGameRoomEventDispatchContent extends Typed
{
    $type: StartGameRoomEventDispatchType;
}

export class StartGameRoomEventDispatch implements GameRoomEventDispatch<StartGameRoomEventDispatchContent>
{
    constructor() {
        this.dispatchKey = StartGameRoomEventDispatchKey;
        this.dispatchFactory = () => {
            return {
                $type: StartGameRoomEventDispatchKey
            }
        }
    }
    
    dispatchKey: string;
    dispatchFactory: () => StartGameRoomEventDispatchContent;
}