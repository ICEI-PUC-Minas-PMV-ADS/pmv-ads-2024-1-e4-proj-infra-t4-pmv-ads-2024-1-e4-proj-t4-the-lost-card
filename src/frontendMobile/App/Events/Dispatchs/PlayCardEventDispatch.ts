import { GameRoomEventDispatch, Typed } from ".."

type PlayCardEventDispatchKeyType = "Application.UseCases.GameRooms.GameActions.PlayCardGameActionRequest, Application";
const PlayCardEventDispatchKey: PlayCardEventDispatchKeyType = "Application.UseCases.GameRooms.GameActions.PlayCardGameActionRequest, Application";

export interface PlayCardEventDispatchContent extends Typed {
    $type: PlayCardEventDispatchKeyType;
    CardId: number;
}

export class PlayCardEventDispatch implements GameRoomEventDispatch<PlayCardEventDispatchContent> {
    constructor(cardId: number) {
        this.dispatchKey = PlayCardEventDispatchKey;
        this.dispatchFactory = () => {
            return {
                $type: PlayCardEventDispatchKey,
                CardId: cardId
            }
        }
    }

    dispatchKey: string;
    dispatchFactory: () => PlayCardEventDispatchContent;
}