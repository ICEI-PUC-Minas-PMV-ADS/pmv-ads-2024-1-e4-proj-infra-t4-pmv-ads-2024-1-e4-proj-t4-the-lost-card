import React, { ReactElement, useContext, createContext, useState } from 'react';
import * as signalR from "@microsoft/signalr";
import AuthContext from './auth';

interface MessagingContextData {
    start(): Promise<signalR.HubConnection>;
}

const MessagingContext = createContext<MessagingContextData>({} as MessagingContextData);

interface MessagingProviderProps extends React.PropsWithChildren {
    children: ReactElement;
}

export const MessagingProvider: React.FC<MessagingProviderProps> = ({ children }) => {
    const { user } = useContext(AuthContext)

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

    return (
        <MessagingContext.Provider value={{ start }}>
            {children}
        </MessagingContext.Provider>
    );
}

export default MessagingContext;