import { GameRoomEventListener, Typed } from "..";
import { GameRoomData } from "../../Context/gameRoom";

type DamagedRecievedEventListenerKeyType = "Application.UseCases.GameRooms.GameEvents.DamageRecievedNotificationDispatch, Application";
const DamagedRecievedEventListenerKey: DamagedRecievedEventListenerKeyType = "Application.UseCases.GameRooms.GameEvents.DamageRecievedNotificationDispatch, Application";

export interface DamagedRecievedEventListenerContent extends Typed {
    $type: DamagedRecievedEventListenerKeyType;
    PlayerName: string;
    BlockDamageAmount: number;
    LifeDamageAmount: number;
    UpdatedCurrentLife: number;
    UpdatedCurrentBlock: number;
    WasKilled: boolean;
}

export interface TextEffect {
    xCord: number | null;
    yCord: number | null;
    text: string;
    color: string;
    isShowing: boolean;
}

export class DamagedRecievedEventListener extends GameRoomEventListener<DamagedRecievedEventListenerContent> {
    constructor(setRoom: React.Dispatch<React.SetStateAction<GameRoomData | null>>, setTextEffects: React.Dispatch<React.SetStateAction<TextEffect[]>>) {
        const onTrigger = (eventContet: DamagedRecievedEventListenerContent) => {
            setRoom(roomDispatch => {
                const playerTargetDict = roomDispatch!.players.map((anyPlayer, index) => {
                    return {
                        isTarget:
                            anyPlayer.name == eventContet.PlayerName,
                        player: anyPlayer,
                        index: index,
                    };
                });

                const damagedPlayer = playerTargetDict.filter(x => x.isTarget)[0];
                damagedPlayer.player.healthPoints = eventContet.UpdatedCurrentLife;
                damagedPlayer.player.blockPoints = eventContet.UpdatedCurrentBlock;
                damagedPlayer.player.isDead = eventContet.WasKilled;

                if (eventContet.LifeDamageAmount > 0)
                    setTextEffects(currentEffects => [
                        ...currentEffects,
                        {
                            xCord: null,
                            yCord: null,
                            text: "-" + eventContet.LifeDamageAmount,
                            color: "red",
                            isShowing: false
                        }
                    ])

                if (eventContet.BlockDamageAmount > 0)
                    setTextEffects(currentEffects => [
                        ...currentEffects,
                        {
                            xCord: null,
                            yCord: null,
                            text: "-" + eventContet.BlockDamageAmount,
                            color: "grey",
                            isShowing: false
                        }
                    ])

                return {
                    ...roomDispatch!,
                    players: [...roomDispatch!.players.filter((_, index) => index != damagedPlayer.index), damagedPlayer.player]
                }
            });
        }

        super(DamagedRecievedEventListenerKey, onTrigger);
    }
}