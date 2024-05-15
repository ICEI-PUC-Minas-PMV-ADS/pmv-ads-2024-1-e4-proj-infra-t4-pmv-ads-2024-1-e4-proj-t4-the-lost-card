import {createNativeStackNavigator} from '@react-navigation/native-stack';
import { Text, View, TextStyle } from "react-native";
// import MainMenuPage from '../Pages/MainMenuPage';
// import QueryRoomsPage from '../Pages/QueryRooms';
// import CreateRoom from '../Pages/CreateRoom';

const {Screen, Navigator} = createNativeStackNavigator();

const AppRoutes = () => {
  return (
    <Navigator screenOptions={{ headerShown: false }}>
      <Screen name="Default" component={DefaultAppRoute} /> 
      {/* <Screen name="MainMenuPage" component={MainMenuPage} />
      <Screen name="QueryRooms" component={QueryRoomsPage} />
      <Screen name="CreateRoom" component={CreateRoom} /> */}
    </Navigator>
  );
};
 
const DefaultAppRoute: React.FC = () => {
  return (
    <View style={{gap: 10, flexDirection: 'row', alignItems: 'center' }}>
      <Text style={contrastTextStyle}>{'\u2022'}</Text>
      <Text style={contrastTextStyle}>Tela default logado</Text>
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

export default AppRoutes;
