import { createNativeStackNavigator } from '@react-navigation/native-stack';
import { Text, View, TextStyle } from "react-native";
// import SignInPage from '../Pages/SignInPage';
// import SplashPage from '../Pages/SplashPage';
// import HomePage from '../Pages/HomePage';
// import SignUpPage from '../Pages/SignUpPage';

const {Screen, Navigator} = createNativeStackNavigator();

const AnonRoutes = () => {
    return (
        <Navigator screenOptions={{ headerShown: false }}>
            <Screen name="Default" component={DefaultAppRoute} /> 
            {/* <Screen name="SplashPage" component={SplashPage} />
            <Screen name="HomePage" component={HomePage} />
            <Screen name="SignInPage" component={SignInPage} />
            <Screen name="SingUpPage" component={SignUpPage} /> */}
        </Navigator>
    )
}

const DefaultAppRoute: React.FC = () => {
    return (
      <View style={{gap: 10, flexDirection: 'row', alignItems: 'center' }}>
        <Text style={contrastTextStyle}>{'\u2022'}</Text>
        <Text style={contrastTextStyle}>Tela default anon</Text>
      </View>
    );
  };
  
  const contrastTextStyle : Readonly<TextStyle> = {
    color: '#DAD8D8',
    fontSize: 35,
    textShadowColor: '#0149BF',
    textShadowOffset: {width: -1, height: 1},
    textShadowRadius: 10,
  }

export default AnonRoutes;