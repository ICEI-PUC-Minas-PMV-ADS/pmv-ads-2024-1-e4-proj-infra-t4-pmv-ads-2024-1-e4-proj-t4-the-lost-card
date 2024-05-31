import { GameRoomEventListener } from "..";
import { EnsureListenerType, GameRoomData } from "../../Context/gameRoom";
import { ChooseClassEventListenerKey } from "./ChooseClassEventListener";
import { DamagedRecievedEventListener, DamagedRecievedEventListenerContent, TextEffect } from "./DamageRecievedEventListener";
import { HandShuffledEventListener, HandShuffledEventListenerContent } from "./HandShuffledEventListener";
import { JoinGameRoomEventListeningKey } from "./JoinGameRoomEventListener";
import { PlayerSpawnedEventListener, PlayerSpawnedEventListenerContent } from "./PlayerSpawnedEventListener";
import { OponnentSpawnedEventListener, OponnentSpawnedEventListenerContent } from "./oponnentSpawnedEventListener";

type StartGameRoomEventListenerKeyType = "Application.UseCases.GameRooms.LobbyActions.StartGameRoomHubRequestResponse, Application";
const StartGameRoomEventListenerKey: StartGameRoomEventListenerKeyType = "Application.UseCases.GameRooms.LobbyActions.StartGameRoomHubRequestResponse, Application";

export interface StartGameRoomEventListenerContent {
    RoomId: string;
    AdminName: string;
    $type: StartGameRoomEventListenerKeyType;
    Players: { Name: string; Class: { Name: string; Id: number } | null }[];
}

export class StartGameRoomEventListener extends GameRoomEventListener<StartGameRoomEventListenerContent> {
    constructor(
        setRoom: React.Dispatch<React.SetStateAction<GameRoomData | null>>,
        removeListener: (listeningKey: string) => void,
        setTextEffects: React.Dispatch<React.SetStateAction<TextEffect[]>>,
        ensureListener: EnsureListenerType
    ) {
        const onTrigger = (eventContet: StartGameRoomEventListenerContent) => {
            setRoom(roomDispatch => {
                return { ...roomDispatch!, hasStarted: true };
            });

            removeListener(JoinGameRoomEventListeningKey);
            removeListener(StartGameRoomEventListenerKey);
            removeListener(ChooseClassEventListenerKey);

            const playerSpawnedEventListener = new PlayerSpawnedEventListener(setRoom)
            const oponnentSpawnedEventListener = new OponnentSpawnedEventListener(setRoom);
            const damagedRecievedEventListener = new DamagedRecievedEventListener(setRoom, setTextEffects);
            const handShuffledEventListener = new HandShuffledEventListener(setRoom);
            ensureListener<PlayerSpawnedEventListener, PlayerSpawnedEventListenerContent>(playerSpawnedEventListener);
            ensureListener<OponnentSpawnedEventListener, OponnentSpawnedEventListenerContent>(oponnentSpawnedEventListener);
            ensureListener<DamagedRecievedEventListener, DamagedRecievedEventListenerContent>(damagedRecievedEventListener);
            ensureListener<HandShuffledEventListener, HandShuffledEventListenerContent>(handShuffledEventListener);
        }

        super(StartGameRoomEventListenerKey, onTrigger);
    }
}

