import { Text, View, TextStyle, TextInput, Button } from "react-native";
import React, { useContext, useState } from 'react';
import MessagingContext from "../Context/messaging";

const GameRoomDebugHelper: React.FC = () => {
  const { start } = useContext(MessagingContext);
  const [messagingInput, setMessagingInput] = useState('');
  const [messages, setMessages] = useState<string[]>([]);
  const [connection, setConnection] = useState<signalR.HubConnection | null>(null);

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

    setConnection(connection);
  }

  async function onSendInput() {
    if (!connection) {
      throw new Error("Not yet connected")
    }
    await connection.invoke("OnServerDispatch", messagingInput);
    setMessagingInput('');
  }

  async function onJoinRoom() {
    if (!connection) {
      throw new Error("Not yet connected")
    }
    if (connection == null) {
      throw new Error("Connection is not started");
    }

    connection.on(
      "OnClientDispatch",
      async (rawEvent: any) => {
        console.log(rawEvent);
        const event = JSON.parse(rawEvent);
        if (
          event.$type == "Application.UseCases.GameRooms.Join.JoinGameRoomHubRequestResponse, Application" &&
          "Name" in event
        )
          console.log(`User: ${event.Name} has joined`);
      }
    );

    const joinGameRoomRequest =
    {
      $type: "Application.UseCases.GameRooms.Join.JoinGameRoomHubRequest, Application",
      roomGuid: messagingInput.length > 0 ? messagingInput : null,
      creationOptions: null
    };

    console.log('invoking join room');
    await connection.invoke("OnServerDispatch", JSON.stringify(joinGameRoomRequest));

    setMessagingInput('');
  }

  return (
    <View style={{ gap: 10, flexDirection: 'column', alignItems: 'center' }}>
      <Text style={contrastTextStyle}>{'\u2022' + "Tela default logado"}</Text>
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
        messages.map((message, index) =>
          <Text key={index} style={textStyle}>{'\u2022' + message}</Text>)
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