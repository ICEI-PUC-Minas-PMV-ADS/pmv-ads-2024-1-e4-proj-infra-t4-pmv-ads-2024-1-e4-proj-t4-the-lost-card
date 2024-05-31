import React, { useContext } from 'react';
import GameRoomContext from '../../Context/gameRoom';
import { LobbySearch } from './LobbySearch';
import { Lobby } from './Lobby';
import { GameProper } from './GameProper';

export const GameRoomRouter: React.FC = () => {
  const { room } =
    useContext(GameRoomContext);

  if (room === null) return <LobbySearch />;

  if (room !== null && !room.hasStarted)
    return (
      <Lobby />
    );

  return <GameProper />;
};
