import { AxiosError } from "axios";
import api from "./api";
import { ProblemDetails, ValidationProblemDetails } from "../DTOs/problemdetails";

export async function queryRooms(){
    try{
        const {data} = await api.get<GameRoomReponse[]>('/gamerooms');

        return data
    }
    catch (error) {
        const axiosError = error as AxiosError;
    
        console.log(axiosError);
    
        if (axiosError.status === 400)
          return axiosError.response!.data as ValidationProblemDetails;
    
        return axiosError.response!.data as ProblemDetails;
      }
}

export interface GameRoomReponse{
    roomGuid: string,
    roomName: string,
    currentPlayers: number
}