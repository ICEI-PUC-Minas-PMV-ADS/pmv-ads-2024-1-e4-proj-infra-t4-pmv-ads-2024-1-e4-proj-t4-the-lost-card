import { Text, View, TextStyle, Button } from "react-native";
import React, { useContext, useEffect } from 'react';
import GameRoomContext from "../../Context/gameRoom";
import AuthContext from "../../Context/auth";
import { joinRoomEventKey } from "./LobbySearch";

const classChoosenEventKey = "Application.UseCases.GameRooms.GameActions.ChooseClassGameActionRequestResponse, Application";

const roomStartedEventKey = "Application.UseCases.GameRooms.LobbyActions.StartGameRoomHubRequestResponse, Application";

interface ChooseClassGameActionRequestResponse {
    $type: "Application.UseCases.GameRooms.GameActions.ChooseClassGameActionRequestResponse, Application";
    Name: string;
    GameClassId: number;
    GameClassName: string;
}

export const Lobby: React.FC = () => {
    const { room, hubConnection, setEvents, events, setRoom } = useContext(GameRoomContext);
    const { user } = useContext(AuthContext);

    const canStartRoom = React.useMemo(() => {
        return room?.adminName == user?.name && (room?.players.length ?? 0) > 1 && room?.players.every(p => p.gameClass != null)
    }, [room, user]);

    const classChoosenEventHandler = async (rawEvent: any) => {
        const anyEvent = JSON.parse(rawEvent);
        if (anyEvent.$type == classChoosenEventKey) {
            const chooseClassGameActionRequestResponse = anyEvent as ChooseClassGameActionRequestResponse;
            setRoom(roomDispatch => {
                const player = roomDispatch!.players.filter(x => x.name == chooseClassGameActionRequestResponse.Name)[0];
                player.gameClass = { name: chooseClassGameActionRequestResponse.GameClassName, id: chooseClassGameActionRequestResponse.GameClassId };
                return roomDispatch;
            });
        }
    }

    useEffect(() => {
        if (!events.has(classChoosenEventKey)) {
            setEvents(map => {
                map.set(classChoosenEventKey, classChoosenEventHandler)
                return map;
            })
            hubConnection!.on(
                "OnClientDispatch",
                classChoosenEventHandler
            );
        }
    }, [])

    async function onStartRoom() {
        
        setEvents(map => {
            const joinRoomEventHandler = map.get(joinRoomEventKey)
            if (joinRoomEventHandler) {
                hubConnection!.off(
                    "OnClientDispatch",
                    joinRoomEventHandler
                )
                map.delete(joinRoomEventKey)
            }

            return map;
        })

        setEvents(map => {
            map.delete(classChoosenEventKey)
            return map;
        })

        hubConnection!.off(
            "OnClientDispatch",
            classChoosenEventHandler
        )

        const roomStartedHandler = async (rawEvent: any) => {
            const anyEvent = JSON.parse(rawEvent);
            if (anyEvent.$type == roomStartedEventKey) {
                setRoom(roomDispatch => {
                    if (roomDispatch != null)
                        roomDispatch.hasStarted = true;

                    return roomDispatch;
                });

                hubConnection!.off(
                    "OnClientDispatch",
                    roomStartedHandler
                )
            }
        }

        hubConnection!.on(
            "OnClientDispatch",
            roomStartedHandler
        );
    }

    return (
        <View style={{ gap: 10, flexDirection: 'column', alignItems: 'center' }}>
            <Text style={contrastTextStyle}>{"\u2022 Lobby"}</Text>
            {
                room?.players.map((player, index) => <Text key={index} style={textStyle}>{`\u2022${player.name} with class ${player.gameClass?.name ?? "None"}. Is Me? ${player.isMe}`}</Text>)
            }
            <Button disabled={!canStartRoom} title={"Começar"} onPress={onStartRoom} />
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
