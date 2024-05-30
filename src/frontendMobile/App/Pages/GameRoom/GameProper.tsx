import { Text, View, TextStyle, Button } from "react-native";
import React, { useContext, useEffect, useRef, useState } from 'react';
import GameRoomContext from "../../Context/gameRoom";
import AuthContext from "../../Context/auth";

const oponnentSpawnedEventKey = "Application.UseCases.GameRooms.GameEvents.DamageRecievedNotificationDispatch, Application";

interface OponnentSpawnedEventDispatch {
    $type: "Application.UseCases.GameRooms.GameEvents.DamageRecievedNotificationDispatch, Application";
    GameId: number;
    MaxLife: number;
    CurrentLife: number;
    Intent: { $type: string, Id: number, Type: number, DamageAmount: number | undefined };
}

const handShuffledEventKey = "Application.UseCases.GameRooms.GameEvents.HandShuffledNotificationDispatch, Application";

interface HandShuffledEventDispatch {
    $type: "Application.UseCases.GameRooms.GameEvents.HandShuffledNotificationDispatch, Application";
}

const damagedRecievedEventKey = "Application.UseCases.GameRooms.GameEvents.OponentSpawnedNotificationDispatch, Application";

interface DamagedRecievedEventDispatch {
    $type: "Application.UseCases.GameRooms.GameEvents.OponentSpawnedNotificationDispatch, Application";
    PlayerName: string;
    BlockDamageAmount: number;
    LifeDamageAmount: number;
    UpdatedCurrentLife: number;
    UpdatedCurrentBlock: number;
    WasKilled: boolean;
}

interface TextEffect {
    xCord: number | null;
    yCord: number | null;
    text: string;
    color: string;
    isShowing: boolean;
}

const turnStartedEventKey = "Application.UseCases.GameRooms.GameEvents.TurnStartedNotificationDispatch, Application";

interface TurnStartedEventDispatch {
    $type: "Application.UseCases.GameRooms.GameEvents.TurnStartedNotificationDispatch, Application";
}

export const GameProper: React.FC = () => {
    const firstRender = useRef(true);
    const { room, hubConnection, setEvents, events } = useContext(GameRoomContext);
    const [players, setPlayers] = useState(room!.players);
    const [oponnent, setOponnent] = useState(room!.oponnent);
    const [textEffects, setTextEffects] = useState<TextEffect[]>([]);

    useEffect(() => {
        if (firstRender.current)
            firstRender.current = false;
    });

    useEffect(() => {
        if (firstRender.current)
            return;

        const effectsDisplayDict = textEffects.map((de, index) => {
            return { shouldShow: !de.isShowing, damageEffect: de, index };
        });

        const effectsNotShowing = effectsDisplayDict.filter(effectNotShowing => effectNotShowing.shouldShow);

        if (effectsNotShowing.length > 0) {
            const { damageEffect, index: indexToRemove } = effectsDisplayDict.pop()!;
            damageEffect!.isShowing = true;

            setTimeout(() => {
                setTextEffects(des => des.filter((_, index) => index != indexToRemove));
            }, 2000);
        }
    }, [textEffects]);

    const oponnentSpawnedEventHandler = (rawEvent: any) => {
        const anyEvent = JSON.parse(rawEvent);
        if (anyEvent.$type == oponnentSpawnedEventKey) {
            const oponnentSpawnedEvent = anyEvent as OponnentSpawnedEventDispatch;
            setOponnent(_ => {
                return {
                    healthPoints: oponnentSpawnedEvent.CurrentLife,
                    id: oponnentSpawnedEvent.GameId,
                    maxHealthPoints: oponnentSpawnedEvent.MaxLife,
                    intent: { $type: oponnentSpawnedEvent.Intent.$type, id: oponnentSpawnedEvent.Intent.Id, type: oponnentSpawnedEvent.Intent.Type, damageAmount: oponnentSpawnedEvent.Intent.DamageAmount }
                };
            });
        }
    }

    const damagedRecievedEventHandler = (rawEvent: any) => {
        const anyEvent = JSON.parse(rawEvent);
        if (anyEvent.$type == damagedRecievedEventKey) {
            const damagedRecievedEvent = anyEvent as DamagedRecievedEventDispatch;
            setPlayers(playersDispatch => {
                const playerTargetDict = playersDispatch.map((anyPlayer, index) => {
                    return {
                      isTarget:
                        anyPlayer.name == damagedRecievedEvent.PlayerName,
                      player: anyPlayer,
                      index: index,
                    };
                  });
          
                const damagedPlayer = playerTargetDict.filter(x => x.isTarget)[0];
                damagedPlayer.player.healthPoints = damagedRecievedEvent.UpdatedCurrentLife;
                damagedPlayer.player.blockPoints = damagedRecievedEvent.UpdatedCurrentBlock;
                damagedPlayer.player.isDead = damagedRecievedEvent.WasKilled;

                if (damagedRecievedEvent.LifeDamageAmount > 0)
                    setTextEffects(currentEffects => [
                        ...currentEffects,
                        {
                            xCord: null,
                            yCord: null,
                            text: "-" + damagedRecievedEvent.LifeDamageAmount,
                            color: "red",
                            isShowing: false
                        }
                    ])

                if (damagedRecievedEvent.BlockDamageAmount > 0)
                    setTextEffects(currentEffects => [
                        ...currentEffects,
                        {
                            xCord: null,
                            yCord: null,
                            text: "-" + damagedRecievedEvent.BlockDamageAmount,
                            color: "grey",
                            isShowing: false
                        }
                    ])

                return [...playersDispatch.filter((_,index) => index != damagedPlayer.index), damagedPlayer.player];
            });
        }
    }

    // CARREGAR ESSES EVENTOS ANTES DE INICIAR A SALA
    useEffect(() => {
        if (!events.has(oponnentSpawnedEventKey)) {
            setEvents(map => {
                map.set(oponnentSpawnedEventKey, oponnentSpawnedEventHandler)
                return map;
            })
            hubConnection!.on(
                "OnClientDispatch",
                oponnentSpawnedEventHandler
            );
        }

        if (!events.has(damagedRecievedEventKey)) {
            setEvents(map => {
                map.set(damagedRecievedEventKey, damagedRecievedEventHandler)
                return map;
            })
            hubConnection!.on(
                "OnClientDispatch",
                damagedRecievedEventHandler
            );
        }
    }, [])

    return (
        <View style={{ gap: 10, flexDirection: 'column', alignItems: 'center' }}>
            <Text style={contrastTextStyle}>{"\u2022 Players"}</Text>
            {
                players.map((player, index) => <Text key={index} style={textStyle}>{`\u2022${player.name}| Class: ${player.gameClass!.name} | Life: ${player.healthPoints}/${player.maxHealthPoints} | Block: ${player.blockPoints} | Is Me? ${player.isMe}`}</Text>)
            }
            {
                oponnent ? <Text style={contrastTextStyle}>{`\u2022 Oponent: ${oponnent.id} | Life: ${oponnent.healthPoints}/${oponnent.maxHealthPoints} | Intent: ${oponnent.intent.type == 1 ? "Attack" : ""} ${oponnent.intent.damageAmount ? `x${oponnent.intent.damageAmount}` : ""}`}</Text> : <></>
            }
            {
                textEffects.length > 0? <Text style={contrastTextStyle}>{"\u2022 Danos"}</Text> : <></>
            }
            {
                textEffects.filter(x => x.isShowing).map((textEffect, index) => <Text key={index} style={{ ...textStyle, color: textEffect.color }}>{`\u2022${textEffect.text}`}</Text>)
            }
        </View>
    );
}

const contrastTextStyle: Readonly<TextStyle> = {
    color: 'red',
    fontSize: 35,
    textShadowColor: '#0149BF',
    textShadowOffset: { width: -1, height: 1 },
    textShadowRadius: 10,
}

const textStyle: Readonly<TextStyle> = {
    color: 'red',
    fontSize: 12,
    textShadowColor: '#0149BF',
    textShadowOffset: { width: -1, height: 1 },
    textShadowRadius: 10,
}