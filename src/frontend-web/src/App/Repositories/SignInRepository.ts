import axios, { AxiosError } from "axios";
import { ProblemDetails, ValidationProblemDetails } from "../DTOs/problemdetails";
import api from "./api";
import { Console } from "console";

export interface SignInRequest {
  email: string;
  plainTextPassword: string;
}

export interface SignInResponse {
  token: string;
  name: string;
}

export async function signIn(request: SignInRequest) {
  try {
    const { data } = await axios.post<SignInResponse>(
      "http://localhost:7097/api/players/sessions",
      request
    );

    return data;
  } catch (error) {
    const axiosError = error as AxiosError;

    console.log(axiosError);

    if (axiosError.status === 404) 
        return axiosError.response!.data as ValidationProblemDetails;

    return axiosError.response!.data as ProblemDetails;
  }
}