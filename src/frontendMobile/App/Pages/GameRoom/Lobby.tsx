import { Text, View, TextStyle, Button } from "react-native";
import React, { useContext } from 'react';
import GameRoomContext from "../../Context/gameRoom";
import AuthContext from "../../Context/auth";

export const Lobby: React.FC = () => {
    const { room } = useContext(GameRoomContext);
    const { user } = useContext(AuthContext);

    const canStartRoom = React.useMemo(() => {
        return room?.adminName == user?.name && (room?.players.length ?? 0) > 1 && room?.players.every(p => p.gameClass != null)
    }, [room, user]);

    return (
        <View style={{ gap: 10, flexDirection: 'column', alignItems: 'center' }}>
            <Text style={contrastTextStyle}>{'\u2022' + "Lobby"}</Text>
            {
                room?.players.map((player, index) => <Text key={index} style={textStyle}>
                    {`\u2022${player.name} with class ${player.gameClass?.name ?? "None"}. Is Me? ${player.isMe}`}
                </Text>)
            }
            <Button disabled={!canStartRoom} title={"Começar"} />
        </View>
    );
}

const contrastTextStyle: Readonly<TextStyle> = {
    color: 'red',
    fontSize: 35,
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
