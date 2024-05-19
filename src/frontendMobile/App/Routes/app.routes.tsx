import { createNativeStackNavigator } from '@react-navigation/native-stack';
import GameRoomDebugHelper from '../Pages/GameRoomDebugHelper';
import { GameRoomRouter } from "../Pages/GameRoom"
// import MainMenuPage from '../Pages/MainMenuPage';
// import QueryRoomsPage from '../Pages/QueryRooms';
// import CreateRoom from '../Pages/CreateRoom';

const { Screen, Navigator } = createNativeStackNavigator();

const AppRoutes = () => {
  return (
    <Navigator screenOptions={{ headerShown: false }} initialRouteName='DebugHelper'>
      <Screen name="DebugHelper" component={GameRoomDebugHelper} />
      <Screen name="GameRoomRouter" component={GameRoomRouter}></Screen>
      {/* <Screen name="MainMenuPage" component={MainMenuPage} />
      <Screen name="QueryRooms" component={QueryRoomsPage} />
      <Screen name="CreateRoom" component={CreateRoom} /> */}
    </Navigator>
  );
};

export default AppRoutes;
