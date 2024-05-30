import React from 'react';
import {
  BackHandler,
  Image,
  Platform,
  Pressable,
  StyleSheet,
  View,
} from 'react-native';
import Anchor from '../Components/Anchor';
import Background from '../Components/Background';
import ListItem from '../Components/ListItem';

const HomePageLoged: React.FC = () => {
  const exit = () => {
    if (Platform.OS === 'android') {
      if (BackHandler) {
        BackHandler.exitApp();
      }
    }
  };

  return (
    <Background>
      <Image source={require('../Assets/Logo.png')} />

      <View style={styles.menuOptions}>
        <Anchor text="Buscar salas" route="GameRoomRouter" />
        <Pressable onPress={exit}>
          <ListItem text="Sair" />
        </Pressable>
      </View>
    </Background>
  );
};

const styles = StyleSheet.create({
  menuOptions: {
    position: 'absolute',
    bottom: 0,
    left: 0,
  },
});

export default HomePageLoged;
