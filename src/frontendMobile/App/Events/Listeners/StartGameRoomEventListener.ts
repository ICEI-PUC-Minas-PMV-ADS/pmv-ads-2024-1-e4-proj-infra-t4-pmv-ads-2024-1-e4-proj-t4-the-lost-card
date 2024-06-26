import { GameRoomEventListener } from "..";
import { EnsureListenerType, GameRoomData } from "../../Context/gameRoom";
import { ChooseClassEventListenerKey } from "./ChooseClassEventListener";
import { HandShuffledEventListener, HandShuffledEventListenerContent } from "./HandShuffledEventListener";
import { JoinGameRoomEventListeningKey } from "./JoinGameRoomEventListener";
import { OponentStatusUpdatedEventListener, OponentStatusUpdatedEventListenerContent } from "./OponentStatusUpdatedEventListener";
import { OponnentSpawnedEventListener, OponnentSpawnedEventListenerContent } from "./OponnentSpawnedEventListener";
import { PlayCardEventListener, PlayCardEventListenerContent } from "./PlayCardEventListener";
import { PlayerSpawnedEventListener, PlayerSpawnedEventListenerContent } from "./PlayerSpawnedEventListener";
import { PlayerStatusUpdatedEventListener, PlayerStatusUpdatedEventListenerContent } from "./PlayerStatusUpdatedEventListener";
import { EndTurnEventListener, EndTurnEventListenerContent } from "./EndTurnEventListener";
import { TurnStartedEventListener, TurnStartedEventListenerContent } from "./TurnStartedEventListener";

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
            const playerStatusUpdatedEventListener = new PlayerStatusUpdatedEventListener(setRoom);
            const handShuffledEventListener = new HandShuffledEventListener(setRoom);
            const playCardEventListener = new PlayCardEventListener(setRoom);
            const oponentStatusUpdatedEventListener = new OponentStatusUpdatedEventListener(setRoom);
            const turnStartedEventListener = new TurnStartedEventListener(setRoom);
            const endTurnEventListener = new EndTurnEventListener(setRoom);
            ensureListener<PlayerSpawnedEventListener, PlayerSpawnedEventListenerContent>(playerSpawnedEventListener);
            ensureListener<OponnentSpawnedEventListener, OponnentSpawnedEventListenerContent>(oponnentSpawnedEventListener);
            ensureListener<PlayerStatusUpdatedEventListener, PlayerStatusUpdatedEventListenerContent>(playerStatusUpdatedEventListener);
            ensureListener<HandShuffledEventListener, HandShuffledEventListenerContent>(handShuffledEventListener);
            ensureListener<PlayCardEventListener, PlayCardEventListenerContent>(playCardEventListener)
            ensureListener<OponentStatusUpdatedEventListener, OponentStatusUpdatedEventListenerContent>(oponentStatusUpdatedEventListener)
            ensureListener<TurnStartedEventListener, TurnStartedEventListenerContent>(turnStartedEventListener)
            ensureListener<EndTurnEventListener, EndTurnEventListenerContent>(endTurnEventListener)
        
        }

        super(StartGameRoomEventListenerKey, onTrigger);
    }
}

