import {
  Text,
  View,
  TextStyle,
  Button,
  Pressable,
  StyleSheet,
} from 'react-native';
import React, {useContext, useEffect, useRef, useState} from 'react';
import GameRoomContext, {GetActualDescription} from '../../Context/gameRoom';
import {TextEffect} from '../../Events/Listeners/PlayerStatusUpdatedEventListener';
import {PlayCardEventDispatch} from '../../Events/Dispatchs/PlayCardEventDispatch';
import Player from '../../Components/Player';
import Background from '../../Components/Background';
import Icons from '../../Components/ClassIcon';
import Card from '../../Components/Card';
import CardBase from '../../Components/CardBase';

interface GameProperProps {
  textEffects: TextEffect[];
  setTextEffects: React.Dispatch<React.SetStateAction<TextEffect[]>>;
}

export const GameProper: React.FC<GameProperProps> = ({
  textEffects,
  setTextEffects,
}) => {
  const firstRender = useRef(true);
  const {room, dispatch} = useContext(GameRoomContext);

  const me = room?.players.find(player => player.isMe);

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
            name="oponente"
            currentLife={room.oponnent.CurrentLife}
            maxLife={room.oponnent.MaxLife}
            currentBlock={room.oponnent.CurrentBlock}>
            <Text style={styles.enimy}>💀</Text>
          </Player>
        ) : (
          <></>
        )}
      </View>
      <View style={styles.cards}>
        {me ? (
          <>
            <CardBase>
              <Text>Pilha</Text>
              <Text>{me.DiscardPile.length}</Text>
            </CardBase>
            <View style={styles.deck}>
              {me.Hand.map((card, index) => (
                <Pressable
                  key={index}
                  onPress={async () =>
                    await dispatch(new PlayCardEventDispatch(card.Id), null)
                  }>
                  <Card
                    title={card.Name}
                    energy={card.EnergyCost}
                    description={GetActualDescription(card)}
                  />
                </Pressable>
              ))}
            </View>

            <CardBase>
              <Text>Descarte</Text>
              <Text>{me.DrawPile.length}</Text>
            </CardBase>
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
