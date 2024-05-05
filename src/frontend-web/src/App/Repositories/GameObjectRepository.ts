import { AxiosError } from "axios";
import { ProblemDetails, ValidationProblemDetails } from "../DTOs/problemdetails";
import api from "./api";

export interface AchivementsResponse {
    $values: Achivement[];
}

export interface Achivement {
    Id: number;
    Title: string;
    Description: string;
    IconPath: string;
}

export async function queryAchievments() {
    try {
        const { data } = await api.get<AchivementsResponse>(
            "/gameobjects/Domain.GameObjects.Achievements+Achievment"
        );

        return data;
    } catch (error) {
        const axiosError = error as AxiosError;

        console.log(axiosError);

        if (axiosError.status === 400)
            return axiosError.response!.data as ValidationProblemDetails;

        return axiosError.response!.data as ProblemDetails;
    }
}
