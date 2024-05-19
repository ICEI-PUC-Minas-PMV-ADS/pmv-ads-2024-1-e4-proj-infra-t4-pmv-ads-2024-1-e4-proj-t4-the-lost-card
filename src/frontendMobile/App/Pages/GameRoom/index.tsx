import React, { useContext } from 'react';
import GameRoomContext from '../../Context/gameRoom';
import { LobbySearch } from './LobbySearch';
import { Lobby } from './Lobby';

export const GameRoomRouter: React.FC = () => {
    const { hubConnection, room } = useContext(GameRoomContext);

    if (!hubConnection && !room)
        return (<LobbySearch />);

    if (!(room!.hasStarted))
        return (<Lobby />);

    return (
        <>
        </>
    );
};