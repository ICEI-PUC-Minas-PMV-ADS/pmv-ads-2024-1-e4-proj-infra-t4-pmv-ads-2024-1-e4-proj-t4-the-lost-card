import { Text, View, TextStyle, TextInput, Button } from "react-native";
import React, { useContext, useState } from 'react';
import GameRoomContext, { GameRoomPlayerData } from "../../Context/gameRoom";
import AuthContext from "../../Context/auth";


interface JoinGameRoomHubRequestResponse {
    RoomId: string,
    AdminName: string ;
    $type: "Application.UseCases.GameRooms.Join.JoinGameRoomHubRequestResponse, Application",
    Players: { Name: string, Class: { Name: string, Id: number } | null }[]
}

export const LobbySearch: React.FC = () => {
    const [roomIdInput, setRoomIdInput] = useState('');
    const { start, setRoom } = useContext(GameRoomContext);
    const { user } = useContext(AuthContext);

    async function onJoinRoom() {
        const connection = await start();
        if (!connection) {
            throw new Error("Connection is not started");
        }

        const handlerBody = async (rawEvent: any) => {
            const anyEvent = JSON.parse(rawEvent);
            if (anyEvent.$type == "Application.UseCases.GameRooms.Join.JoinGameRoomHubRequestResponse, Application") {
                const joinResponse = anyEvent as JoinGameRoomHubRequestResponse;
                setRoom({
                    id: joinResponse.RoomId,
                    adminName: joinResponse.AdminName,
                    hasStarted: false,
                    players: joinResponse.Players.map(x => new GameRoomPlayerData(
                        x.Name,
                        x.Name == user?.name,
                        x.Class ? { name: x.Class.Name, id: x.Class.Id } : null
                    ))
                });
                connection.off("OnClientDispatch", handlerBody)
            }
        }

        connection.on(
            "OnClientDispatch",
            handlerBody
        );

        const joinGameRoomRequest =
        {
            $type: "Application.UseCases.GameRooms.Join.JoinGameRoomHubRequest, Application",
            roomGuid: roomIdInput.length > 0 ? roomIdInput : null,
            creationOptions: null
        };

        await connection.invoke("OnServerDispatch", JSON.stringify(joinGameRoomRequest));

        setRoomIdInput('');
    }

    return (
        <View style={{ gap: 10, flexDirection: 'column', alignItems: 'center' }}>
            <Text style={contrastTextStyle}>{'\u2022' + "Pesquisa de lobies"}</Text>
            <TextInput
                onChangeText={e => {
                    setRoomIdInput(e);
                }}
                value={roomIdInput}
                placeholder={"ID DA SALA"}
                style={{ width: '100%', color: "black" }}
            />
            <Button onPress={onJoinRoom} title={"Entrar em sala"} />
        </View>
    );
};

const contrastTextStyle: Readonly<TextStyle> = {
    color: 'red',
    fontSize: 35,
    textShadowColor: '#0149BF',
    textShadowOffset: { width: -1, height: 1 },
    textShadowRadius: 10,
}