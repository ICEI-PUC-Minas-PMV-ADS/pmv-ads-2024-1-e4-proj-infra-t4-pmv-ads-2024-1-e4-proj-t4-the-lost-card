import {
  Text,
  View,
  Pressable,
  StyleSheet,
  ToastAndroid,
} from 'react-native';
import React, { useContext } from 'react';
import GameRoomContext, { GetActualDescription } from '../../Context/gameRoom';
import { PlayCardEventDispatch } from '../../Events/Dispatchs/PlayCardEventDispatch';
import Player from '../../Components/Player';
import Background from '../../Components/Background';
import Icons from '../../Components/ClassIcon';
import Card from '../../Components/Card';
import CardBase from '../../Components/CardBase';
import { EndTurnDispatch } from '../../Events/Dispatchs/EndTurnDispatch';

export const GameProper: React.FC = () => {
  const { room, dispatch } = useContext(GameRoomContext);

  const me = React.useMemo(() => {
    return room?.players.find(player => player.isMe);
  }, [room]);

  async function onPlayCard(cardId: number, energyCost: number) {
    if ((me?.CurrentEnergy ?? 0) < energyCost) {
      ToastAndroid.show("Nao tem energia suficiente", 2000);
    }
    else if (me?.ActionsFinished ?? true) {
      ToastAndroid.show("Seu turno acabou", 2000);
    } else {
      await dispatch(new PlayCardEventDispatch(cardId), null)
    }
  }

  return (
    <Background>
      <View style={styles.battleField}>
        {room?.players.map((player, index) => (
          <Player
            key={index}
            name={player.name}
            currentLife={player.CurrentLife}
            maxLife={player.MaxLife}
            currentBlock={player.CurrentBlock}
            energyCurrent={player.CurrentEnergy}
            maxEnergy={player.MaxEnergy}>
            {Icons.get(player.gameClass!.id)}
          </Player>
        ))}
        {room?.oponnent ? (
          <Player
            name={`oponente lvl.${room.currentLevel}`}
            currentLife={room.oponnent.CurrentLife}
            maxLife={room.oponnent.MaxLife}
            currentBlock={room.oponnent.CurrentBlock}>
            <View style={{ flexDirection: 'row' }}>
              <Text style={styles.enimy}>💀</Text>
              {room.oponnent.intent.type == 1 ? <>
                <Text style={styles.enimy}>{`${room.oponnent.intent.damageAmount}x`}</Text>
                <Text style={styles.enimy}>🗡️</Text>
              </> : <></>}
            </View>
          </Player>
        ) : (
          <></>
        )}
      </View>
      <View style={styles.cards}>
        {me ? (
        <>
            <CardBase scaleFactor={0.85}>
              <Text>Descarte</Text>
              <Text>{me.DiscardPile.length}</Text>
            </CardBase>
            <View style={styles.deck}>
              {me.Hand.map((card, index) => (
                <Pressable
                  key={index}
                  onPress={async () => await onPlayCard(card.Id, card.EnergyCost)}>
                  <Card
                    title={card.Name}
                    energy={card.EnergyCost}
                    description={GetActualDescription(card)}
                  />
                </Pressable>
              ))}
            </View>

            <View style={{ flexDirection: "column", flexWrap: 'nowrap', justifyContent: 'flex-end', alignItems: 'center', alignContent: 'flex-end' }}>
              <Pressable
                onPress={async () =>
                  await dispatch(new EndTurnDispatch(), null)
                }>
                <Text>Terminar turno</Text>
              </Pressable>
              <CardBase scaleFactor={0.85}>
                <Text>Pilha</Text>
                <Text>{me.DrawPile.length}</Text>
              </CardBase>
            </View>
          </>
        ) : (
          <></>
        )}
      </View>
    </Background>
  );
};

const styles = StyleSheet.create({
  cards: {
    position: 'absolute',
    bottom: 0,
    width: '100%',
    justifyContent: 'space-between',
    alignItems: 'flex-end',
    flexDirection: 'row',
  },
  deck: {
    justifyContent: 'center',
    flexDirection: 'row',
    alignItems: 'center',
    gap: 5,
  },
  enimy: {
    borderRadius: 100,
    backgroundColor: 'black',
    width: 40,
    height: 40,
    textAlign: 'center',
    fontSize: 25,
  },
  battleField: {
    flexDirection: 'row',
    gap: 70,
  },
});
