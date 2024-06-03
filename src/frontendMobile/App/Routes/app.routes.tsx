import { NativeStackNavigationProp, createNativeStackNavigator } from '@react-navigation/native-stack';
import { GameRoomRouter } from "../Pages/GameRoom"
import { LobbySearch } from '../Pages/GameRoom/LobbySearch';
import { Lobby } from '../Pages/GameRoom/Lobby';
import HomePageLoged from '../Pages/HomePageLoged';
// import MainMenuPage from '../Pages/MainMenuPage';
// import QueryRoomsPage from '../Pages/QueryRooms';
import CreateRoom from '../Pages/CreateRoom';

export type StackParamList = {
  HomePage: undefined;
  LobbySearch: undefined;
  GameRoomRouter: undefined;
  CreateRoomRouter: undefined;
};

export type StackNavigation = NativeStackNavigationProp<StackParamList>;

const { Screen, Navigator } = createNativeStackNavigator<StackParamList>();

const AppRoutes = () => {
  return (
    <Navigator screenOptions={{ headerShown: false }} initialRouteName='HomePage'>
      <Screen name="LobbySearch" component={LobbySearch} />
      <Screen name="GameRoomRouter" component={GameRoomRouter} />
      <Screen name="HomePage" component={HomePageLoged} />
      <Screen name='CreateRoomRouter' component={CreateRoom}/>
    </Navigator>
  );
};

export default AppRoutes;
