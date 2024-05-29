import { Text, View, TextStyle, Button } from "react-native";
import React, { useContext, useEffect } from 'react';
import GameRoomContext from "../../Context/gameRoom";
import AuthContext from "../../Context/auth";

const oponnentSpawnedEventKey = "Application.UseCases.GameRooms.GameEvents.DamageRecievedNotificationDispatch, Application";

interface OponnentSpawnedEventKeyResponse {
    $type: "Application.UseCases.GameRooms.GameEvents.DamageRecievedNotificationDispatch, Application";
    GameId: number;
    MaxLife: number;
    CurrentLife: number;
    Intent: { $type: string, Id: number, Type: number };
}

const handShuffledEventKey = "Application.UseCases.GameRooms.GameEvents.HandShuffledNotificationDispatch, Application";
const onDamagedRecievedEventKey = "Application.UseCases.GameRooms.GameEvents.OponentSpawnedNotificationDispatch, Application";
const onTurnStartedEventKey = "Application.UseCases.GameRooms.GameEvents.TurnStartedNotificationDispatch, Application";

export const GameProper: React.FC = () => {
    const { room, hubConnection, setEvents, events, setRoom } = useContext(GameRoomContext);
    const { user } = useContext(AuthContext);

    const oponnentSpawnedEventHandler = (rawEvent: any) => {
        const anyEvent = JSON.parse(rawEvent);
        if (anyEvent.$type == oponnentSpawnedEventKey) {
            const oponnentSpawnedEvent = anyEvent as OponnentSpawnedEventKeyResponse;
            setRoom(roomDispatch => {
                if (roomDispatch != null)
                    roomDispatch.oponnent = {
                        healthPoints: oponnentSpawnedEvent.CurrentLife,
                        id: oponnentSpawnedEvent.GameId,
                        maxHealthPoints: oponnentSpawnedEvent.MaxLife,
                        intent: { $type: oponnentSpawnedEvent.Intent.$type, id: oponnentSpawnedEvent.Intent.Id, type: oponnentSpawnedEvent.Intent.Type }
                    };

                return roomDispatch;
            });
        }
    }

    const onDamagedRecievedEventHandler = (rawEvent: any) => {
        const anyEvent = JSON.parse(rawEvent);
        if (anyEvent.$type == oponnentSpawnedEventKey) {
        
        }
    }

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
    }, [])

    return (
        <View style={{ gap: 10, flexDirection: 'column', alignItems: 'center' }}>
            <Text style={contrastTextStyle}>{"\u2022 Lobby"}</Text>
            {
                room?.players.map((player, index) => <Text key={index} style={textStyle}>{`\u2022${player.name} with class ${player.gameClass?.name ?? "None"}. Is Me? ${player.isMe}`}</Text>)
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