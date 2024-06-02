import React, { ReactElement, useContext, createContext, useState, useEffect } from 'react';
import * as signalR from "@microsoft/signalr";
import AuthContext from './auth';
import { GameRoomEventDispatch, GameRoomEventListener, GameRoomEventListenerBase, Typed } from "../Events"

export type EnsureListenerType = <TListener extends GameRoomEventListener<TListenerContent>, TListenerContent extends Typed>(listener: TListener) => void;
class GameRoomPlayerData {
    constructor(name: string, isMe: boolean, gameClass: { name: string, id: number } | null) {
        this.name = name;
        this.isMe = isMe;
        this.gameClass = gameClass;
        this.CurrentLife = Number.MIN_SAFE_INTEGER;
        this.MaxLife = Number.MIN_SAFE_INTEGER;
        this.CurrentBlock = 0;
        this.CurrentEnergy = Number.MIN_SAFE_INTEGER;
        this.MaxEnergy = Number.MIN_SAFE_INTEGER;
        this.Hand = [];
        this.DrawPile = [];
        this.DiscardPile = [];
    }

    [key: string]: any;
    name: string;
    gameClass: { name: string, id: number } | null;
    isMe: boolean;
    CurrentLife: number;
    MaxLife: number;
    CurrentBlock: number;
    CurrentEnergy: number;
    MaxEnergy: number;
    Hand: Card[];
    DrawPile: Card[];
    DiscardPile: Card[];
}

export interface Card {
    Id: number;
    Name: string;
    EnergyCost: number;
    Description: string;
    $type: string;
}

export interface OponnentData {
    [key: string]: any;

    id: number;
    MaxLife: number;
    CurrentLife: number;
    CurrentBlock: number;
    intent: { $type: string, id: number, type: number, damageAmount: number | undefined }
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
    room: GameRoomData | null;
    setRoom: React.Dispatch<React.SetStateAction<GameRoomData | null>>;
    dispatch<TDispatch extends GameRoomEventDispatch<TDispatchContent>, TDispatchContent extends Typed>(dispatch: TDispatch, connection: signalR.HubConnection | null | undefined): Promise<void>;
    ensureListener: EnsureListenerType;
    removeListener(listeningKey: string): void;
}

const GameRoomContext = createContext<GameRoomContextData>({} as GameRoomContextData);

interface GameRoomContextProviderProps extends React.PropsWithChildren {
    children: ReactElement;
}

export const GameRoomContextProvider: React.FC<GameRoomContextProviderProps> = ({ children }) => {
    const { user } = useContext(AuthContext)
    const [hubConnection, setHubConnection] = useState<signalR.HubConnection | null>(null);
    const [gameRoom, setGameRoom] = useState<GameRoomData | null>(null);
    const [eventListeners, setEventListeners] = useState<{ listener: GameRoomEventListenerBase, listening: boolean, actualOnTrigger: (event: string) => void }[]>([]);

    function removeListener(listeningKey: string) {
        setEventListeners(eventListeners => {
            const eventsToRemoveDict = eventListeners.map((x, index) => {
                return {
                    isTarget: x.listener.listeningKey == listeningKey && x.listening,
                    value: x,
                    index: index,
                };
            });

            if (eventsToRemoveDict.length > 0)
                hubConnection!.off("OnClientDispatch", eventsToRemoveDict[0].value.actualOnTrigger)

            return eventsToRemoveDict.filter(x => !x.isTarget).map(x => x.value);
        });
    }

    const ensureListener: EnsureListenerType = (listener) => {
        if (eventListeners.every(x => x.listener.listeningKey !== listener.listeningKey))
            setEventListeners(eventListeners => {
                const actualOnTrigger = (rawEvent: string) => {
                    const anyEvent = JSON.parse(rawEvent);
                    if (anyEvent.$type == listener.listeningKey)
                        listener.onTrigger(anyEvent)
                }
                return [...eventListeners, { listener, listening: false, actualOnTrigger }]
            });
    }

    useEffect(() => {
        const eventsNotListened = eventListeners.filter(x => !x.listening);
        if (eventsNotListened.length > 0) {
            setEventListeners(eventListeners => {
                const eventsToListenDict = eventListeners.map((listener, index) => {
                    return {
                        isTarget: !listener.listening,
                        value: listener,
                        index: index,
                    };
                });

                return [
                    ...eventsToListenDict.filter(x => !x.isTarget).map(x => x.value),
                    ...eventsToListenDict.filter(x => x.isTarget).map(x => {
                        hubConnection!.on('OnClientDispatch', x.value.actualOnTrigger)
                        return { listener: x.value.listener, listening: true, actualOnTrigger: x.value.actualOnTrigger }
                    })
                ];
            });
        }
    }, [eventListeners]);

    async function dispatch<TDispatch extends GameRoomEventDispatch<TDispatchContent>, TDispatchContent extends Typed>(dispatch: TDispatch, connection: signalR.HubConnection | null | undefined) {
        await (connection ? connection : hubConnection!).invoke("OnServerDispatch", JSON.stringify(dispatch.dispatchFactory()));
    }

    async function start(): Promise<signalR.HubConnection> {
        if (!user) {
            throw new Error("Not yet logged");
        }

        if (hubConnection) {
            return hubConnection;
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
        return await (connection.start()
            .then(() => {
                console.log('connected!');
                setHubConnection(connection);
                return connection;
            })
            .catch((err) => {
                throw err;
            }));
    }

    return (
        <GameRoomContext.Provider
            value={{
                start,
                room: gameRoom,
                setRoom: setGameRoom,
                dispatch,
                ensureListener,
                removeListener
            }}
        >
            {children}
        </GameRoomContext.Provider>
    );
}

export default GameRoomContext;

export { GameRoomPlayerData }