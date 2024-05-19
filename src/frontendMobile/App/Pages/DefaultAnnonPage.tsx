import { Text, View, TextStyle, TextInput, Button } from "react-native";
import React, { useContext, useState } from 'react';
import AuthContext from "../Context/auth";

const DefaultAnnonPage: React.FC = () => {
  const [tokenInput, setTokenInput] = useState('');
  const { setToken } = useContext(AuthContext)

  function onPress() {
    setToken(tokenInput)
  }

  return (
    <View style={{ gap: 10, flexDirection: 'column', alignItems: 'center' }}>
      <Text style={contrastTextStyle}>{'\u2022'}</Text>
      <Text style={contrastTextStyle}>Tela default anon</Text>
      <TextInput
        onChangeText={e => {
          setTokenInput(e);
        }}
        value={tokenInput}
        placeholder={"Token"}
        style={{width: '100%', color: "black"}}
      />
      <Button onPress={onPress} title={"Usar token"} />
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

export default DefaultAnnonPage;