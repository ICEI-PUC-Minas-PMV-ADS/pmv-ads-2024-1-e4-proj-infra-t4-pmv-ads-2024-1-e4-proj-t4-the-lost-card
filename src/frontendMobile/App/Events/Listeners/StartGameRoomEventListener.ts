import { GameRoomEventListener } from "..";
import { GameRoomData } from "../../Context/gameRoom";
import { ChooseClassEventListenerKey } from "./ChooseClassEventListener";
import { JoinGameRoomEventListeningKey } from "./JoinGameRoomEventListener";

type StartGameRoomEventListenerKeyType = "Application.UseCases.GameRooms.LobbyActions.StartGameRoomHubRequestResponse, Application";
const StartGameRoomEventListenerKey: StartGameRoomEventListenerKeyType = "Application.UseCases.GameRooms.LobbyActions.StartGameRoomHubRequestResponse, Application";

export interface StartGameRoomEventListenerContent {
    RoomId: string;
    AdminName: string;
    $type: StartGameRoomEventListenerKeyType;
    Players: { Name: string; Class: { Name: string; Id: number } | null }[];
}

export class StartGameRoomEventListener extends GameRoomEventListener<StartGameRoomEventListenerContent> {
    constructor(setRoom: React.Dispatch<React.SetStateAction<GameRoomData | null>>, removeListener: (listeningKey: string) => void) {
        const onTrigger = (eventContet: StartGameRoomEventListenerContent) => {
            setRoom(roomDispatch => {
                return { ...roomDispatch!, hasStarted: true };
            });

            removeListener(JoinGameRoomEventListeningKey);
            removeListener(StartGameRoomEventListenerKey);
            removeListener(ChooseClassEventListenerKey);
        }

        super(StartGameRoomEventListenerKey, onTrigger);
    }
}

