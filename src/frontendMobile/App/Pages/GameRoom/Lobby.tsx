import {
  View,
  StyleSheet,
  Text,
  TouchableOpacity,
} from 'react-native';
import React, { useContext, useEffect, useState } from 'react';
import GameRoomContext from '../../Context/gameRoom';
import AuthContext from '../../Context/auth';
import { joinRoomEventKey } from './LobbySearch';
import Background from '../../Components/Background';
import Kawasaki from '../../Components/Kawasaki';
import WarriorIcon from '../../Assets/Warrior.svg';
import LostCardButton from '../../Components/Button';

const classChoosenEventKey =
  'Application.UseCases.GameRooms.GameActions.ChooseClassGameActionRequestResponse, Application';

const roomStartedEventKey =
  'Application.UseCases.GameRooms.LobbyActions.StartGameRoomHubRequestResponse, Application';

interface ChooseClassGameActionRequestResponse {
  $type: 'Application.UseCases.GameRooms.GameActions.ChooseClassGameActionRequestResponse, Application';
  Name: string;
  GameClassId: number;
  GameClassName: string;
}

export const Lobby: React.FC = () => {
  const { events, hubConnection, room, setEvents, setRoom } = useContext(GameRoomContext);
  const [players, setPlayers] = useState(room!.players);

  const canStartRoom = React.useMemo(() => {
    return (
      room?.adminName == room?.players.filter(x => x.isMe)[0] &&
      (room?.players.length ?? 0) > 1 &&
      room?.players.every(p => p.gameClass != null)
    );
  }, [room]);

  const classChoosenEventHandler = async (rawEvent: any) => {
    console.log('callback ativo');
    const anyEvent = JSON.parse(rawEvent);
    if (anyEvent.$type == classChoosenEventKey) {
      console.log('chamando evento de classe escolhida');
      const chooseClassGameActionRequestResponse =
        anyEvent as ChooseClassGameActionRequestResponse;

      setPlayers(playersDispatch => {
        const playerTargetDict = playersDispatch.map((anyPlayer, index) => {
          return {
            isTarget: anyPlayer.name == chooseClassGameActionRequestResponse.Name,
            player: anyPlayer,
            index: index,
          };
        });

        const targetPlayer = playerTargetDict.filter(x => x.isTarget)[0];

        targetPlayer.player.gameClass = {
          name: chooseClassGameActionRequestResponse.GameClassName,
          id: chooseClassGameActionRequestResponse.GameClassId,
        };

        return [
          ...playersDispatch.filter(x => x != targetPlayer.player),
          targetPlayer.player,
        ];
      });
    }
  };

  const roomStartedHandler = async (rawEvent: any) => {
    const anyEvent = JSON.parse(rawEvent);
    if (anyEvent.$type == roomStartedEventKey) {
      setEvents(map => {
        map.delete(joinRoomEventKey);
        return map;
      });

      setRoom(roomDispatch => {
        return {...roomDispatch!, hasStarted: true};
      });

      hubConnection!.off('OnClientDispatch', roomStartedHandler);
    }
  };

  async function onStartRoom() {
    setEvents(map => {
      map.delete(classChoosenEventKey);
      return map;
    });

    hubConnection!.off('OnClientDispatch', classChoosenEventHandler);

    const startGameRoom = {
      $type:
        'Application.UseCases.GameRooms.LobbyActions.StartGameRoomHubRequest, Application',
    };

    await hubConnection!.invoke(
      'OnServerDispatch',
      JSON.stringify(startGameRoom),
    );
  }

  useEffect(() => {
    if (!events.has(classChoosenEventKey)) {
      setEvents(map => {
        map.set(classChoosenEventKey, classChoosenEventHandler);
        return map;
      });
      hubConnection!.on('OnClientDispatch', classChoosenEventHandler);
    }
    if (!events.has(joinRoomEventKey)) {
      setEvents(map => {
        map.set(joinRoomEventKey, roomStartedHandler)
        return map;
      });
      hubConnection!.on('OnClientDispatch', roomStartedHandler);
    }
  }, []);

  const classChoosen = (id: number) => {
    const chooseClass = {
      $type:
        'Application.UseCases.GameRooms.GameActions.ChooseClassGameActionRequest, Application',
      GameClassId: id,
    };

    hubConnection?.invoke('OnServerDispatch', JSON.stringify(chooseClass));
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
          {players.map((player, index) => (
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
