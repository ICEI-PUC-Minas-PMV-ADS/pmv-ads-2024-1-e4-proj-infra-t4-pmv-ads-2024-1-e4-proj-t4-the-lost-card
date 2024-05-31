import { Text, View, TextStyle, Button } from "react-native";
import React, { useContext, useEffect, useRef, useState } from 'react';
import GameRoomContext from "../../Context/gameRoom";
import { TextEffect } from "../../Events/Listeners/DamageRecievedEventListener";

interface GameProperProps {
    textEffects: TextEffect[],
    setTextEffects: React.Dispatch<React.SetStateAction<TextEffect[]>>
}

export const GameProper: React.FC<GameProperProps> = ({ textEffects, setTextEffects }) => {
    const firstRender = useRef(true);
    const { room } = useContext(GameRoomContext);

    useEffect(() => {
        if (firstRender.current)
            firstRender.current = false;
    });

    useEffect(() => {
        if (firstRender.current)
            return;

        const effectsDisplayDict = textEffects.map((de, index) => {
            return { shouldShow: !de.isShowing, damageEffect: de, index };
        });

        const effectsNotShowing = effectsDisplayDict.filter(effectNotShowing => effectNotShowing.shouldShow);

        if (effectsNotShowing.length > 0) {
            const { damageEffect, index: indexToRemove } = effectsDisplayDict.pop()!;
            damageEffect!.isShowing = true;

            setTimeout(() => {
                setTextEffects(des => des.filter((_, index) => index != indexToRemove));
            }, 2000);
        }
    }, [textEffects]);

    return (
        <View style={{ gap: 10, flexDirection: 'column', alignItems: 'center' }}>
            <Text style={contrastTextStyle}>{"\u2022 Players"}</Text>
            {
                room?.players.map((player, index) => {

                    return (<>
                        <Text key={index} style={textStyle}>{`\u2022${player.name}| Class: ${player.gameClass!.name} | Life: ${player.healthPoints}/${player.maxHealthPoints} | Block: ${player.blockPoints}`}</Text>
                        {player.isMe ? <>
                            <Text key={index * (room?.players.length + 1)} style={textStyle}>{`\u2022 Mão:${player.hand.map((card, cardIndex) => ` ${card.Id}`)}`}</Text>
                            <Text key={index * (room?.players.length + 1) + 1} style={textStyle}>{`\u2022 Fuça:${player.drawPile.map((card, cardIndex) => ` ${card.Id}`)}`}</Text>
                            <Text key={index * (room?.players.length + 1) + 2} style={textStyle}>{`\u2022 Descarte${player.discardPile.map((card, cardIndex) => ` ${card.Id}`)}`}</Text>
                        </> : <></>}
                    </>)
                })
            }
            {
                room?.oponnent ? <Text style={contrastTextStyle}>{`\u2022 Oponent: ${room?.oponnent.id} | Life: ${room?.oponnent.healthPoints}/${room?.oponnent.maxHealthPoints} | Intent: ${room?.oponnent.intent.type == 1 ? "Attack" : ""} ${room?.oponnent.intent.damageAmount ? `x${room?.oponnent.intent.damageAmount}` : ""}`}</Text> : <></>
            }
            {
                textEffects.length > 0 ? <Text style={contrastTextStyle}>{"\u2022 Danos"}</Text> : <></>
            }
            {
                textEffects.filter(x => x.isShowing).map((textEffect, index) => <Text key={index} style={{ ...textStyle, color: textEffect.color }}>{`\u2022${textEffect.text}`}</Text>)
            }
        </View>
    );
}

const contrastTextStyle: Readonly<TextStyle> = {
    color: 'red',
    fontSize: 30,
    textShadowColor: '#0149BF',
    textShadowOffset: { width: -1, height: 1 },
    textShadowRadius: 10,
}

const textStyle: Readonly<TextStyle> = {
    color: 'red',
    fontSize: 12,
    textShadowColor: '#0149BF',
    textShadowOffset: { width: -1, height: 1 },
    textShadowRadius: 10,
}