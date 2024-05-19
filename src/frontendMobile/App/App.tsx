import React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { AuthProvider } from './Context/auth';
import { MessagingProvider } from './Context/messaging';
import Routes from './Routes';
import 'react-native-url-polyfill/auto';

function App(): JSX.Element {
  return (
    <NavigationContainer>
      <AuthProvider>
        <MessagingProvider>
          <Routes />
        </MessagingProvider>
      </AuthProvider>
    </NavigationContainer>
  );
}

export default App;
