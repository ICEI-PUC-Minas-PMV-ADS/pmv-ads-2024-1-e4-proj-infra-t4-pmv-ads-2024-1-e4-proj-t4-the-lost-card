import React, { useContext, useState } from 'react';
import GameRoomContext from '../../Context/gameRoom';
import { LobbySearch } from './LobbySearch';
import { Lobby } from './Lobby';
import { GameProper } from './GameProper';
import { TextEffect } from '../../Events/Listeners/PlayerStatusUpdatedEventListener';

export const GameRoomRouter: React.FC = () => {
  const { room } = useContext(GameRoomContext);
  const [textEffects, setTextEffects] = useState<TextEffect[]>([]);


  if (room === null) return <LobbySearch />;

  if (room !== null && !room.hasStarted)
    return (
      <Lobby setTextEffects={setTextEffects} />
    );

  return <GameProper textEffects={textEffects} setTextEffects={setTextEffects} />;
};
