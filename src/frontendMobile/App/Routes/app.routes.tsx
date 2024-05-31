import { createNativeStackNavigator } from '@react-navigation/native-stack';
import { GameRoomRouter } from "../Pages/GameRoom"
import { LobbySearch } from '../Pages/GameRoom/LobbySearch';
import { Lobby } from '../Pages/GameRoom/Lobby';
import HomePageLoged from '../Pages/HomePageLoged';
// import MainMenuPage from '../Pages/MainMenuPage';
// import QueryRoomsPage from '../Pages/QueryRooms';
// import CreateRoom from '../Pages/CreateRoom';

const { Screen, Navigator } = createNativeStackNavigator();

const AppRoutes = () => {
  return (
    <Navigator screenOptions={{ headerShown: false }} initialRouteName='HomePage'>
      <Screen name="LobbySearch" component={LobbySearch} />
      <Screen name="GameRoomRouter" component={GameRoomRouter} />
      <Screen name="HomePage" component={HomePageLoged} />
    </Navigator>
  );
};

export default AppRoutes;
