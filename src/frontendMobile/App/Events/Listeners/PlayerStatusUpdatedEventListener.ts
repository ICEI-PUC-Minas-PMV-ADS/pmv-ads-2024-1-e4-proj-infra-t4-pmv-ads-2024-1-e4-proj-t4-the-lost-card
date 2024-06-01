import { GameRoomEventListener, Typed } from "..";
import { GameRoomData } from "../../Context/gameRoom";

type PlayerStatusUpdatedEventListenerKeyType = "Application.UseCases.GameRooms.GameEvents.PlayerStatusUpdatedNotificationDispatch, Application";
export const PlayerStatusUpdatedEventListenerKey: PlayerStatusUpdatedEventListenerKeyType = "Application.UseCases.GameRooms.GameEvents.PlayerStatusUpdatedNotificationDispatch, Application"

const statusEmojiDict = new Map<string, { emoji: string, color: string }>([
    ["CurrentEnergy", { emoji: "⚡", color: "yellow" }],
    ["CurrentLife", { emoji: "🩸", color: "red" }],
    ["CurrentBlock", { emoji: "🛡️", color: "grey" }]
])

export interface PlayerStatusUpdatedEventListenerContent extends Typed {
    $type: PlayerStatusUpdatedEventListenerKeyType;
    PlayerName: string;
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

export class PlayerStatusUpdatedEventListener extends GameRoomEventListener<PlayerStatusUpdatedEventListenerContent> {
    constructor(setRoom: React.Dispatch<React.SetStateAction<GameRoomData | null>>, setTextEffects: React.Dispatch<React.SetStateAction<TextEffect[]>>) {
        const onTrigger = (eventContet: PlayerStatusUpdatedEventListenerContent) => {
            setRoom(roomDispatch => {
                const playerTargetDict = roomDispatch!.players.map((anyPlayer, index) => {
                    return {
                        isTarget: anyPlayer.name == eventContet.PlayerName,
                        player: anyPlayer,
                        index: index,
                    };
                });

                const targetedPlayer = playerTargetDict.filter(x => x.isTarget)[0];
                targetedPlayer.player[eventContet.StatusName as keyof typeof targetedPlayer.player] = eventContet.FreshValue;

                setTextEffects(currentEffects => [
                    ...currentEffects,
                    {
                        xCord: null,
                        yCord: null,
                        text: `${eventContet.PlayerName}'s ${statusEmojiDict.get(eventContet.StatusName)!.emoji} went from ${eventContet.StaleValue} to ${eventContet.FreshValue}`,
                        color: statusEmojiDict.get(eventContet.StatusName)!.color,
                        isShowing: false
                    }
                ])

                return {
                    ...roomDispatch!,
                    players: [...roomDispatch!.players.filter((_, index) => index != targetedPlayer.index), targetedPlayer.player]
                }
            });
        }

        super(PlayerStatusUpdatedEventListenerKey, onTrigger);
    }
}