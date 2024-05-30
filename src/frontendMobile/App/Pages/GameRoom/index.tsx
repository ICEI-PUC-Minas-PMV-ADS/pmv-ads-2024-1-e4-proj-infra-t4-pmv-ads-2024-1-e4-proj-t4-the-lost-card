import React, {useContext} from 'react';
import GameRoomContext from '../../Context/gameRoom';
import {LobbySearch} from './LobbySearch';
import {Lobby} from './Lobby';

export const GameRoomRouter: React.FC = () => {
  const {hubConnection, room, events, setEvents, setRoom} =
    useContext(GameRoomContext);

  if (hubConnection === null && room === null) return <LobbySearch />;

  if (room !== null && !room.hasStarted)
    return (
      <Lobby />
    );

  return <></>;
};
