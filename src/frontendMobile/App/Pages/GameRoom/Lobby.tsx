import {
  View,
  StyleSheet,
  Text,
  TouchableOpacity,
} from 'react-native';
import React, { useContext, useEffect } from 'react';
import GameRoomContext from '../../Context/gameRoom';
import Background from '../../Components/Background';
import Kawasaki from '../../Components/Kawasaki';
import WarriorIcon from '../../Assets/Warrior.svg';
import LostCardButton from '../../Components/Button';
import { ChooseClassEventListener, ChooseClassEventListenerContent } from '../../Events/Listeners/ChooseClassEventListener';
import { StartGameRoomEventListener, StartGameRoomEventListenerContent } from '../../Events/Listeners/StartGameRoomEventListener';
import { StartGameRoomEventDispatch } from '../../Events/Dispatchs/StartGameRoomEventDispatch';
import { ChooseClassEventDispatch } from '../../Events/Dispatchs/ChooseClassEventDispatch';
import { TextEffect } from '../../Events/Listeners/DamageRecievedEventListener';

interface LobbyProps {
  setTextEffects: React.Dispatch<React.SetStateAction<TextEffect[]>>
}

export const Lobby: React.FC<LobbyProps> = ({setTextEffects}) => {
  const { room, setRoom, ensureListener, removeListener, dispatch } = useContext(GameRoomContext);

  const canStartRoom = React.useMemo(() => {
    return (
      room?.adminName == room?.players.filter(x => x.isMe)[0] &&
      (room?.players.length ?? 0) > 1 &&
      room?.players.every(p => p.gameClass != null)
    );
  }, [room]);

  const chooseClassEventListener = new ChooseClassEventListener(setRoom);
  const roomStartedEventListener = new StartGameRoomEventListener(setRoom, removeListener, setTextEffects, ensureListener);

  useEffect(() => {
    ensureListener<ChooseClassEventListener, ChooseClassEventListenerContent>(chooseClassEventListener);
    ensureListener<StartGameRoomEventListener, StartGameRoomEventListenerContent>(roomStartedEventListener);
  }, [])

  async function onStartRoom() {
    const startRoomDispatch = new StartGameRoomEventDispatch();

    await dispatch(startRoomDispatch, null);
  }

  async function classChoosen(id: number) {
    const chooseClassDispatch = new ChooseClassEventDispatch(id);

    await dispatch(chooseClassDispatch, null);
  };

  const Warrior = () => {
    return (
      <View style={{ justifyContent: 'center' }}>
        <WarriorIcon width={40} />
      </View>
    );
  };

  const icons = new Map<number, JSX.Element>([[15977656387767, Warrior()]]);

  return (
    <Background>
      <View style={styles.container}>
        <View style={{ gap: 15, alignItems: 'flex-start', height: '80%' }}>
          {room?.players.map((player, index) => (
            <Kawasaki
              key={index}
              text={`${player.name} ${player.isMe ? '<-' : ''}`}>
              {player.gameClass?.id ? icons.get(player.gameClass?.id) : null}
            </Kawasaki>
          ))}
        </View>
        <View style={styles.verticleLine} />
        <View
          style={{
            alignItems: 'flex-start',
            height: '80%',
            justifyContent: 'space-between',
          }}>
          <View>
            <Text style={{ fontSize: 24 }}>Escolher classe:</Text>
            <View
              style={{
                justifyContent: 'center',
                alignItems: 'center',
                flexDirection: 'row',
              }}>
              <TouchableOpacity onPress={() => classChoosen(15977656387767)}>
                <Warrior />
              </TouchableOpacity>
            </View>
          </View>
          <LostCardButton
            text="Começar"
            disabled={canStartRoom}
            onPress={onStartRoom}
          />
        </View>
      </View>
    </Background>
  );
};

const styles = StyleSheet.create({
  container: {
    width: '80%',
    height: '75%',
    flexDirection: 'row',
    justifyContent: 'space-around',
    alignItems: 'center',
    backgroundColor: '#5E5C72',
    borderRadius: 10,
    padding: 20,
  },
  verticleLine: {
    height: '90%',
    width: 1,
    backgroundColor: '#909090',
  },
});
