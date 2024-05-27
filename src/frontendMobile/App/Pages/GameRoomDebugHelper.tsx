import { Text, View, TextStyle, TextInput, Button } from "react-native";
import React, { useContext, useState } from 'react';
import GameRoomContext from "../Context/gameRoom";

const GameRoomDebugHelper: React.FC = () => {
  const { start, hubConnection } = useContext(GameRoomContext);
  const [messagingInput, setMessagingInput] = useState('');
  const [messages, setMessages] = useState<string[]>([]);

  async function onStartConnection() {
    const connection = await start();

    connection.on(
      "OnClientDispatch",
      (rawEvent: any) => {
        console.log('recieved event in old connection: ');
        console.log(rawEvent);
        setMessages(currentMessages => [...currentMessages, rawEvent]);
      }
    )
  }

  async function onSendInput() {
    if (!hubConnection) {
      throw new Error("Not yet connected")
    }
    await hubConnection.invoke("OnServerDispatch", messagingInput);
    setMessagingInput('');
  }

  async function onJoinRoom() {
    if (!hubConnection) {
      throw new Error("Connection is not started");
    }

    hubConnection.on(
      "OnClientDispatch",
      async (rawEvent: string) => {
        console.log(rawEvent);
        const event = JSON.parse(rawEvent);
        if (
          event.$type == "Application.UseCases.GameRooms.LobbyActions.JoinGameRoomHubRequestResponse, Application" &&
          "Name" in event
        )
          console.log(`User: ${event.Name} has joined`);
      }
    );

    const joinGameRoomRequest =
    {
      $type: "Application.UseCases.GameRooms.LobbyActions.JoinGameRoomHubRequest, Application",
      roomGuid: messagingInput.length > 0 ? messagingInput : null,
      creationOptions: null
    };

    console.log('invoking join room');
    await hubConnection.invoke("OnServerDispatch", JSON.stringify(joinGameRoomRequest));

    setMessagingInput('');
  }

  return (
    <View style={{ gap: 10, flexDirection: 'column', alignItems: 'center' }}>
      <Text style={contrastTextStyle}>{"\u2022 Tela default logado"}</Text>
      <Button onPress={onStartConnection} title={"Iniciar conexao"} />
      <TextInput
        onChangeText={e => {
          setMessagingInput(e);
        }}
        value={messagingInput}
        placeholder={"Input de texto"}
        style={{ width: '100%', color: "black" }}
      />
      <Button onPress={onSendInput} title={"Enviar texto"} />
      <Button onPress={onJoinRoom} title={"Entrar em sala"} />
      {
        messages.map((message, index) => <Text key={index} style={textStyle}>{'\u2022' + message}</Text>)
      }
    </View>
  );
};

const contrastTextStyle: Readonly<TextStyle> = {
  color: '#DAD8D8',
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

export default GameRoomDebugHelper;