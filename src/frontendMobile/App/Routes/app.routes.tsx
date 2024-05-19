import { createNativeStackNavigator } from '@react-navigation/native-stack';
import { Text, View, TextStyle } from "react-native";
import DefaultAppRoute from '../Pages/DefaultAppPage';
// import MainMenuPage from '../Pages/MainMenuPage';
// import QueryRoomsPage from '../Pages/QueryRooms';
// import CreateRoom from '../Pages/CreateRoom';

const { Screen, Navigator } = createNativeStackNavigator();

const AppRoutes = () => {
  return (
    <Navigator screenOptions={{ headerShown: false }} initialRouteName='DefaultApp'>
      <Screen name="DefaultApp" component={DefaultAppRoute} />
      {/* <Screen name="MainMenuPage" component={MainMenuPage} />
      <Screen name="QueryRooms" component={QueryRoomsPage} />
      <Screen name="CreateRoom" component={CreateRoom} /> */}
    </Navigator>
  );
};

export default AppRoutes;
