import React, { ReactElement, useContext, createContext, useState } from 'react';
import * as signalR from "@microsoft/signalr";
import AuthContext from './auth';

class GameRoomPlayerData {
    constructor(name: string, isMe: boolean, gameClass: { name: string, id: number } | null) {
        this.name = name;
        this.isMe = isMe;
        this.gameClass = gameClass;
        this.healthPoints = Number.MIN_SAFE_INTEGER;
        this.maxHealthPoints = Number.MIN_SAFE_INTEGER;
        this.hand = [];
        this.drawPile = [];
        this.discardPile = [];
    }

    name: string;
    gameClass: { name: string, id: number } | null;
    isMe: boolean;
    healthPoints: number;
    maxHealthPoints: number;
    hand: Card[];
    drawPile: Card[];
    discardPile: Card[];
}

interface Card {
    Id: number;
    $type: string;
}

interface OponnentData {
    id: number;
    maxHealthPoints: number;
    healthPoints: number;
    intent: { $type: string, id: number, type: number } 
}

export interface GameRoomData {
    id: string;
    adminName: string;
    hasStarted: boolean;
    players: GameRoomPlayerData[];
    oponnent: OponnentData | null
}

interface GameRoomContextData {
    start(): Promise<signalR.HubConnection>;
    hubConnection: signalR.HubConnection | null;
    room: GameRoomData | null;
    setRoom: React.Dispatch<React.SetStateAction<GameRoomData | null>>;
    setEvents: React.Dispatch<React.SetStateAction<Map<string, (...args: any[]) => void>>>;
    events: Map<string, (...args: any[]) => void>;
}

const GameRoomContext = createContext<GameRoomContextData>({} as GameRoomContextData);

interface GameRoomContextProviderProps extends React.PropsWithChildren {
    children: ReactElement;
}

export const GameRoomContextProvider: React.FC<GameRoomContextProviderProps> = ({ children }) => {
    const { user } = useContext(AuthContext)
    const [hubConnection, setHubConnection] = useState<signalR.HubConnection | null>(null);
    const [gameRoom, setGameRoom] = useState<GameRoomData | null>(null);
    const [events, setEvents] = useState<Map<string, (...args: any[]) => void>>(new Map<string, (...args: any[]) => void>());

    async function start(): Promise<signalR.HubConnection> {
        if (!user) {
            throw new Error("Not yet logged");
        }

        if (hubConnection) {
            throw new Error("Already connected");
        }

        console.log(`creating connection... with token: ${user.token}`);
        const connection = new signalR
            .HubConnectionBuilder()
            .withUrl("https://lost-cards-devfunc.azurewebsites.net/api", {
                accessTokenFactory: () => {
                    return user?.token ?? "";
                }
            })
            .configureLogging(signalR.LogLevel.Information)
            .build();

        connection.on('OnClientDispatch', (rawEvent) => {
            console.log("Recieved event: ")
            console.log(rawEvent)
        });

        console.log('starting connection...');
        return await connection.start()
            .then(() => {
                console.log('connected!');
                setHubConnection(connection);
                return connection;
            })
            .catch((err) => {
                throw err;
            });
    }

    return (
        <GameRoomContext.Provider
            value={{
                start,
                hubConnection,
                room: gameRoom,
                setEvents,
                setRoom: setGameRoom,
                events
            }}
        >
            {children}
        </GameRoomContext.Provider>
    );
}

export default GameRoomContext;

export { GameRoomPlayerData }