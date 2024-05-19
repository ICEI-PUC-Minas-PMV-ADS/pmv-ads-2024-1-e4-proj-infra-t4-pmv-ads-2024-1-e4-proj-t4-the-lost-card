import React, { ReactElement, useContext, createContext, useState } from 'react';
import * as signalR from "@microsoft/signalr";
import AuthContext from './auth';
import PromiseExtensions from "../Extensions/PromiseExtensions"
import uuidExtensions from '../Extensions/uuidExtensions';

interface MessagingContextData {
    start(): Promise<signalR.HubConnection>;
    joinRoom(roomGuid: string | null,  connection: signalR.HubConnection): Promise<void>
    newConnection: signalR.HubConnection | null;
    // addCallback(eventMatchPredicate: (event: any) => boolean, callback: (((event: any) => void) | ((event: string) => Promise<void>))): () => void;
}

interface JoinRoomResponse { newToken: string, name: string };

const MessagingContext = createContext<MessagingContextData>({} as MessagingContextData);

interface MessagingProviderProps extends React.PropsWithChildren {
    children: ReactElement;
}

export const MessagingProvider: React.FC<MessagingProviderProps> = ({ children }) => {
    const { user, setToken } = useContext(AuthContext)
    const [newConnection, setNewConnection] = useState<signalR.HubConnection|null>(null);

    async function start(): Promise<signalR.HubConnection> {
        console.log(`creating connection... with token: ${user?.token ?? ""}`);
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
                return connection;
            })
            .catch((err) => { 
                throw err;
            });
    }

    // function addCallback(eventMatchPredicate: (event: any) => boolean, eventHandler: (((event: any) => void) | ((event: string) => Promise<void>))): () => void {
    //     const callbackKey = uuidExtensions.generateUUID()
    //     setCallbackMap(map => [...map, { callbackKey, eventMatchPredicate, eventHandler }])
    //     return () => setCallbackMap(map => map.filter(x => x.callbackKey != callbackKey));
    // }

    // TODO: Adicionar opcoes de criacao
    async function joinRoom(roomGuid: string | null, connection: signalR.HubConnection) {
        console.log('joining room');

        if (connection == null) {
            throw new Error("Connection is not started");
        }

        console.log("adding callback");

        connection.on(
            "OnClientDispatch",
            async (rawEvent: any) => {
                console.log('refresh callback recieved');
                console.log(`currentUser: ${(user?.name ?? "")}`);
                console.log(rawEvent);
                const event = JSON.parse(rawEvent);
                if (
                    event.$type == "Application.UseCases.GameRooms.Join.JoinGameRoomHubRequestResponse, Application" && 
                    "NewToken" in event && 
                    "Name" in event && 
                    event.Name == (user?.name ?? "")
                ){
                    const joinRoomResponseValue = { newToken: event.NewToken, name: event.Name };
                    console.log(joinRoomResponseValue)
                    console.log('Setting new token...');
                    setToken(joinRoomResponseValue.newToken);
                    setNewConnection(await start());
                }
            }
        );

        const joinGameRoomRequest =
        {
            $type: "Application.UseCases.GameRooms.Join.JoinGameRoomHubRequest, Application",
            roomGuid: roomGuid,
            creationOptions: null
        };

        console.log('invoking join room');
        await connection.invoke("OnServerDispatch", JSON.stringify(joinGameRoomRequest));
    }

    return (
        <MessagingContext.Provider value={{ start, joinRoom, newConnection }}>
            {children}
        </MessagingContext.Provider>
    );
}

export default MessagingContext;