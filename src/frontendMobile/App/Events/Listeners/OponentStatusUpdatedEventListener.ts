import { GameRoomEventListener, Typed } from "..";
import { GameRoomData, OponnentData } from "../../Context/gameRoom";
import { TextEffect } from "./PlayerStatusUpdatedEventListener";

type OponentStatusUpdatedEventListenerKeyType = "Application.UseCases.GameRooms.GameEvents.OponentStatusUpdatedNotificationDispatch, Application";
export const OponentStatusUpdatedEventListenerKey: OponentStatusUpdatedEventListenerKeyType = "Application.UseCases.GameRooms.GameEvents.OponentStatusUpdatedNotificationDispatch, Application"

const statusEmojiDict = new Map<string, { emoji: string, color: string }>([
    ["CurrentLife", { emoji: "🩸", color: "red" }],
    ["CurrentBlock", { emoji: "🛡️", color: "grey" }]
])

export interface OponentStatusUpdatedEventListenerContent extends Typed {
    $type: OponentStatusUpdatedEventListenerKeyType;
    StatusName: string;
    StaleValue: number;
    FreshValue: number;
}

export class OponentStatusUpdatedEventListener extends GameRoomEventListener<OponentStatusUpdatedEventListenerContent> {
    constructor(setRoom: React.Dispatch<React.SetStateAction<GameRoomData | null>>) {
        const onTrigger = (eventContet: OponentStatusUpdatedEventListenerContent) => {
            setRoom(roomDispatch => {
                roomDispatch!.oponnent![eventContet.StatusName as keyof OponnentData] = eventContet.FreshValue;

                return {
                    ...roomDispatch!,
                    oponnent: roomDispatch!.oponnent!
                }
            });
        }

        super(OponentStatusUpdatedEventListenerKey, onTrigger);
    }
}