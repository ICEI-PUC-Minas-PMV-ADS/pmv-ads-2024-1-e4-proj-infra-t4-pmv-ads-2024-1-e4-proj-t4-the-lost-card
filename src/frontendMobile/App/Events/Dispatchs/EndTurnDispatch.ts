import { GameRoomEventDispatch, Typed } from ".."

type EndTurnDispatchKeyType = "Application.UseCases.GameRooms.GameActions.EndTurnGameActionRequest, Application";
const EndTurnDispatchKey: EndTurnDispatchKeyType = "Application.UseCases.GameRooms.GameActions.EndTurnGameActionRequest, Application";

export interface EndTurnDispatchContent extends Typed {
    $type: EndTurnDispatchKeyType;
}

export class EndTurnDispatch implements GameRoomEventDispatch<EndTurnDispatchContent> {
    constructor() {
        this.dispatchKey = EndTurnDispatchKey;
        this.dispatchFactory = () => {
            return {
                $type: EndTurnDispatchKey
            }
        }
    }

    dispatchKey: string;
    dispatchFactory: () => EndTurnDispatchContent;
}