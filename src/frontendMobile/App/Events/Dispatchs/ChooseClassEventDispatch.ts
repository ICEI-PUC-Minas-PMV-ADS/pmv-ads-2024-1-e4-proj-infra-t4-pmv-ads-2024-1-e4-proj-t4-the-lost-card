import { GameRoomEventDispatch, Typed } from ".."

type ChooseClassEventDispatchType = "Application.UseCases.GameRooms.GameActions.ChooseClassGameActionRequest, Application";
const ChooseClassEventDispatchKey: ChooseClassEventDispatchType = "Application.UseCases.GameRooms.GameActions.ChooseClassGameActionRequest, Application";

export interface ChooseClassEventDispatchContent extends Typed
{
    $type: ChooseClassEventDispatchType;
    GameClassId:  number;
}

export class ChooseClassEventDispatch implements GameRoomEventDispatch<ChooseClassEventDispatchContent>
{
    constructor(classId: number) {
        this.dispatchKey = ChooseClassEventDispatchKey;
        this.dispatchFactory = () => {
            return {
                $type: ChooseClassEventDispatchKey,
                GameClassId: classId
            }
        }
    }
    
    dispatchKey: string;
    dispatchFactory: () => ChooseClassEventDispatchContent;
}