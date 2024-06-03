import {Text, View, TextStyle, Button, Pressable} from 'react-native';
import React, {useContext, useEffect, useRef, useState} from 'react';
import GameRoomContext, { GetActualDescription } from '../../Context/gameRoom';
import {TextEffect} from '../../Events/Listeners/PlayerStatusUpdatedEventListener';
import {PlayCardEventDispatch} from '../../Events/Dispatchs/PlayCardEventDispatch';
import Player from '../../Components/Player';
import Background from '../../Components/Background';
import Icons from '../../Components/ClassIcon';
import Card from '../../Components/Card';

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

  // useEffect(() => {
  //     if (firstRender.current)
  //         firstRender.current = false;
  // });

  // useEffect(() => {
  //     if (firstRender.current)
  //         return;

  //     const effectsDisplayDict = textEffects.map((de, index) => {
  //         return { shouldShow: !de.isShowing, damageEffect: de, index };
  //     });

  //     const effectsNotShowing = effectsDisplayDict.filter(effectNotShowing => effectNotShowing.shouldShow);

  //     if (effectsNotShowing.length > 0) {
  //         const { damageEffect, index: indexToRemove } = effectsDisplayDict.pop()!;
  //         damageEffect!.isShowing = true;

  //         setTimeout(() => {
  //             setTextEffects(des => des.filter((_, index) => index != indexToRemove));
  //         }, 2000);
  //     }
  // }, [textEffects]);

  return (
    <Background>
      {room?.players.map((player, index) => {
        return (
          <>
            <Player
              name={player.name}
              currentLife={player.CurrentLife}
              maxLife={player.MaxLife}
              currentBlock={player.CurrentBlock}
            >
              {Icons.get(player.gameClass!.id)}
            </Player>
            {player.isMe ? (
              <>
                <View
                  key={index * (room?.players.length + 1)}
                  style={{gap: 5, flexDirection: 'row', alignItems: 'center'}}>
                  {player.Hand.map(card => (
                    <Pressable
                      key={card.Id}
                      onPress={async () =>
                        await dispatch(new PlayCardEventDispatch(card.Id), null)
                      }>
                      <Card
                        key={card.Id}
                        title={card.Name}
                        energy={card.EnergyCost}
                        description={GetActualDescription(card)} />
                    </Pressable>
                  ))}
                </View>
                <Text
                  key={index * (room?.players.length + 1) + 1}
                  style={textStyle}>{`\u2022 Fuça:${player.DrawPile.map(
                  card => ` ${card.Name}`,
                )}`}</Text>
                <Text
                  key={index * (room?.players.length + 1) + 2}
                  style={textStyle}>{`\u2022 Descarte:${player.DiscardPile.map(
                  card => ` ${card.Name}`,
                )}`}</Text>
              </>
            ) : (
              <></>
            )}
          </>
        );
      })}
      {room?.oponnent ? (
        <Text style={contrastTextStyle}>{`\u2022 Oponent: ${
          room?.oponnent.id
        } | Life: ${room?.oponnent.CurrentLife}/${
          room?.oponnent.MaxLife
        } | Intent: ${room?.oponnent.intent.type == 1 ? '🗡️' : ''} ${
          room?.oponnent.intent.damageAmount
            ? `x${room?.oponnent.intent.damageAmount}`
            : ''
        }`}</Text>
      ) : (
        <></>
      )}
      {/* {
                textEffects.length > 0 ? <Text style={contrastTextStyle}>{"\u2022 Danos"}</Text> : <></>
            }
            {
                textEffects.filter(x => x.isShowing).map((textEffect, index) => <Text key={index} style={{ ...textStyle, color: textEffect.color }}>{`\u2022${textEffect.text}`}</Text>)
            } */}
    </Background>
  );
};

const contrastTextStyle: Readonly<TextStyle> = {
  color: 'red',
  fontSize: 30,
  textShadowColor: '#0149BF',
  textShadowOffset: {width: -1, height: 1},
  textShadowRadius: 10,
};

const textStyle: Readonly<TextStyle> = {
  color: 'red',
  fontSize: 12,
  textShadowColor: '#0149BF',
  textShadowOffset: {width: -1, height: 1},
  textShadowRadius: 10,
};
