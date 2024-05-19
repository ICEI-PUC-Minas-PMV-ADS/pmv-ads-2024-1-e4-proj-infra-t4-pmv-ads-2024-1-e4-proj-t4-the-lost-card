import React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { AuthProvider } from './Context/auth';
import { GameRoomContextProvider } from './Context/gameRoom';
import Routes from './Routes';
import 'react-native-url-polyfill/auto';

function App(): JSX.Element {
  return (
    <NavigationContainer>
      <AuthProvider>
        <GameRoomContextProvider>
          <Routes />
        </GameRoomContextProvider>
      </AuthProvider>
    </NavigationContainer>
  );
}

export default App;
