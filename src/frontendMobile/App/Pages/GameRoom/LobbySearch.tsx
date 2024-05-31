import {
  ScrollView,
  StyleSheet,
  Text,
  View,
} from 'react-native';
import React, { useContext, useEffect, useState } from 'react';
import GameRoomContext, { GameRoomPlayerData } from '../../Context/gameRoom';
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
import { JoinGameRoomEventListener, JoinGameRoomEventListenerContent } from '../../Events/Listeners/JoinGameRoomEventListener';
import { JoinGameRoomEventDispatch } from '../../Events/Dispatchs/JoinGameRoomEventDispatch';

export const LobbySearch: React.FC = () => {
  const { start, setRoom, dispatch, ensureListener } = useContext(GameRoomContext);
  const { user } = useContext(AuthContext);
  const [rooms, setRooms] = useState<GameRoomReponse[]>([]);

  const [roomNameFilter, setRoomNameFilter] = useState<string>('');

  async function onJoinRoom(id: string) {
    const connection = await start();
    if (!connection) {
      throw new Error('Connection is not started');
    }

    const joinGameRoomListener = new JoinGameRoomEventListener(setRoom, user!.name);

    ensureListener<JoinGameRoomEventListener, JoinGameRoomEventListenerContent>(joinGameRoomListener)

    const joinGameRoomDispatch = new JoinGameRoomEventDispatch(id.length > 0 ? id : null, null);

    dispatch(joinGameRoomDispatch, connection)
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
