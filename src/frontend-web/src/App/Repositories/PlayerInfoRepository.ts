import { AxiosError } from "axios";
import {
  ProblemDetails,
  ValidationProblemDetails,
} from "../DTOs/problemdetails";
import api from "./api";
import { Achivement } from "./GameObjectRepository";

export interface PlayerInfoResponse {
  unlockedAchievments: Achivement[];
}

export async function queryPlayerInfo({ playerId }: { playerId: string }) {
  try {
    const { data } = await api.get<PlayerInfoResponse>(`players/${playerId}`);

    return data;
  } catch (error) {
    const axiosError = error as AxiosError;

    console.log(axiosError);

    if (axiosError.status === 400)
      return axiosError.response!.data as ValidationProblemDetails;

    return axiosError.response!.data as ProblemDetails;
  }
}
