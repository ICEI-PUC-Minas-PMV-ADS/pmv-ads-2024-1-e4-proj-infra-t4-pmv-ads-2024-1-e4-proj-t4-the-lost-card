import { Text, View, TextStyle, TextInput, Button } from "react-native";
import React, { useContext, useEffect, useState } from 'react';
import AuthContext from "../Context/auth";
import MessagingContext from "../Context/messaging";

const DefaultAppRoute: React.FC = () => {
  const { start, joinRoom, newConnection } = useContext(MessagingContext);
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
    await joinRoom(null, connection)
    setMessagingInput('');
  }

  useEffect(() => {
    console.log("Effect was called")
    if(newConnection != null)
    {
      newConnection.on(
        "OnClientDispatch",
        (rawEvent: any) => {
          console.log('recieved event in new connection: ');
          console.log(rawEvent);
          setMessages(currentMessages => [...currentMessages, rawEvent]);
        }
      )

      setConnection(newConnection);
    }
  }, [newConnection])

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

export default DefaultAppRoute;