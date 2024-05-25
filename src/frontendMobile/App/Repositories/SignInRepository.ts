import { AxiosError } from "axios";
import { ProblemDetails, ValidationProblemDetails } from "../DTOs/problemdetails";
import api from "./api";

export interface SignInRequest {
  email: string;
  plainTextPassword: string;
}

export interface SignInResponse {
  token: string;
  name: string;
  id: string;
}

export async function signIn(request: SignInRequest): Promise<SignInResponse | ValidationProblemDetails | ProblemDetails> {
  try {
    const { data } = await api.post<SignInResponse>(
      "/players/sessions",
      request
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
