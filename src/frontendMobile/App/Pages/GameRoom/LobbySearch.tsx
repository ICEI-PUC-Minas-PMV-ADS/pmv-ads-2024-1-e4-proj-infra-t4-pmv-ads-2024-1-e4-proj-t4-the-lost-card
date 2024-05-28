import {
  ScrollView,
  StyleSheet,
  Text,
  TextStyle,
  TouchableOpacity,
  View,
} from 'react-native';
import React, {useContext, useEffect, useState} from 'react';
import GameRoomContext, {GameRoomPlayerData} from '../../Context/gameRoom';
import AuthContext from '../../Context/auth';
import {
  GameRoomReponse,
  queryRooms,
} from '../../Repositories/LobbySearchReposity';
import Kawasaki from '../../Components/Kawasaki';
import Background from '../../Components/Background';
import LostCardModal from '../../Components/Modal';
import Input from '../../Components/Input';
import SearchIcon from '../../Assets/Vector.svg';

interface JoinGameRoomHubRequestResponse {
  RoomId: string;
  AdminName: string;
  $type: 'Application.UseCases.GameRooms.Join.JoinGameRoomHubRequestResponse, Application';
  Players: {Name: string; Class: {Name: string; Id: number} | null}[];
}

export const LobbySearch: React.FC = () => {
  const {start, setRoom} = useContext(GameRoomContext);
  const {user} = useContext(AuthContext);
  const [rooms, setRooms] = useState<GameRoomReponse[]>([]);

  const [roomNameFilter, setRoomNameFilter] = useState<string>('');

  async function onJoinRoom(id: string) {
    const connection = await start();
    if (!connection) {
      throw new Error('Connection is not started');
    }

    const handlerBody = async (rawEvent: any) => {
      const anyEvent = JSON.parse(rawEvent);
      if (
        anyEvent.$type ==
        'Application.UseCases.GameRooms.Join.JoinGameRoomHubRequestResponse, Application'
      ) {
        const joinResponse = anyEvent as JoinGameRoomHubRequestResponse;
        setRoom({
          id: joinResponse.RoomId,
          adminName: joinResponse.AdminName,
          hasStarted: false,
          players: joinResponse.Players.map(
            x =>
              new GameRoomPlayerData(
                x.Name,
                x.Name == user?.name,
                x.Class ? {name: x.Class.Name, id: x.Class.Id} : null,
              ),
          ),
        });
        connection.off('OnClientDispatch', handlerBody);
      }
    };

    connection.on('OnClientDispatch', handlerBody);

    const joinGameRoomRequest = {
      $type:
        'Application.UseCases.GameRooms.Join.JoinGameRoomHubRequest, Application',
      roomGuid: id,
      creationOptions: null,
    };

    await connection.invoke(
      'OnServerDispatch',
      JSON.stringify(joinGameRoomRequest),
    );
  }

  useEffect(() => {
    queryRooms().then(response => {
      if ('title' in response) throw new Error('rooms query failed');

      setRooms(response);
    });
  }, [roomNameFilter]);

  return (
    <Background>
      <LostCardModal style={styles.container}>
        <Input
          placeholder="Search"
          onChangeText={value => setRoomNameFilter(value)}>
          <SearchIcon />
        </Input>
        <ScrollView>
          <View style={styles.lobbyList}>
            {(roomNameFilter === ''
              ? rooms
              : rooms.filter(room => room.roomName.includes(roomNameFilter))
            ).map(room => (
              <Kawasaki
                key={room.roomGuid}
                text={room.roomName}
                onPress={() => onJoinRoom(room.roomGuid)}>
                <Text>{room.currentPlayers}/2</Text>
              </Kawasaki>
            ))}
          </View>
        </ScrollView>
      </LostCardModal>
    </Background>
  );
};

const styles = StyleSheet.create({
  lobbyList: {
    height: '80%',
    gap: 20,
  },
  container: {
    gap: 15,
  },
});
