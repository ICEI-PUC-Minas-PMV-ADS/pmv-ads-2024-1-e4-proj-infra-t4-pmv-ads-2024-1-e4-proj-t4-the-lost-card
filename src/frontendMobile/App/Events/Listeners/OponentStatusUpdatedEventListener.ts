import { GameRoomEventListener, Typed } from "..";
import { GameRoomData, OponnentData } from "../../Context/gameRoom";

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

export interface TextEffect {
    xCord: number | null;
    yCord: number | null;
    text: string;
    color: string;
    isShowing: boolean;
}

export class OponentStatusUpdatedEventListener extends GameRoomEventListener<OponentStatusUpdatedEventListenerContent> {
    constructor(setRoom: React.Dispatch<React.SetStateAction<GameRoomData | null>>, setTextEffects: React.Dispatch<React.SetStateAction<TextEffect[]>>) {
        const onTrigger = (eventContet: OponentStatusUpdatedEventListenerContent) => {
            setRoom(roomDispatch => {
                roomDispatch!.oponnent![eventContet.StatusName as keyof OponnentData] = eventContet.FreshValue;

                setTextEffects(currentEffects => [
                    ...currentEffects,
                    {
                        xCord: null,
                        yCord: null,
                        text: `Oponent's ${statusEmojiDict.get(eventContet.StatusName)!.emoji} went from ${eventContet.StaleValue} to ${eventContet.FreshValue}`,
                        color: statusEmojiDict.get(eventContet.StatusName)!.color,
                        isShowing: false
                    }
                ])

                return {
                    ...roomDispatch!,
                    oponnent: roomDispatch!.oponnent!
                }
            });
        }

        super(OponentStatusUpdatedEventListenerKey, onTrigger);
    }
}