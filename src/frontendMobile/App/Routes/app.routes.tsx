import { createNativeStackNavigator } from '@react-navigation/native-stack';
import GameRoomDebugHelper from '../Pages/GameRoomDebugHelper';
import { GameRoomRouter } from "../Pages/GameRoom"
import { LobbySearch } from '../Pages/GameRoom/LobbySearch';
// import MainMenuPage from '../Pages/MainMenuPage';
// import QueryRoomsPage from '../Pages/QueryRooms';
// import CreateRoom from '../Pages/CreateRoom';

const { Screen, Navigator } = createNativeStackNavigator();

const AppRoutes = () => {
  return (
    <Navigator screenOptions={{ headerShown: false }} initialRouteName='GameRoomRouter'>
      <Screen name="DebugHelper" component={GameRoomDebugHelper} />
      <Screen name="LobbySearch" component={LobbySearch} />
      <Screen name="GameRoomRouter" component={GameRoomRouter}></Screen>
      {/* <Screen name="MainMenuPage" component={MainMenuPage} />
      <Screen name="QueryRooms" component={QueryRoomsPage} />
      <Screen name="CreateRoom" component={CreateRoom} /> */}
    </Navigator>
  );
};

export default AppRoutes;
