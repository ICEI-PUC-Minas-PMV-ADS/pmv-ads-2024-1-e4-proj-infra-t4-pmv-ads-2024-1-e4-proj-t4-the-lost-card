import { AxiosError } from "axios";
import { ProblemDetails, ValidationProblemDetails } from "../DTOs/problemdetails";
import api from "./api";


export interface SignUpRequest {
    name: string;
    email: string;
    plainTextPassword: string;
  }

export interface SignUpResponse {
    id: string;
  }


export async function signUp(request: SignUpRequest): Promise<SignUpResponse | ValidationProblemDetails | ProblemDetails> {
    try {
      const { data } = await api.post<SignUpResponse>(
        "/players",
        request
      );
      
     return data;
    } 
    catch (error) {
      const axiosError = error as AxiosError;
  
      console.log(axiosError);
  
      if (axiosError.status === 400) 
        return axiosError.response!.data as ValidationProblemDetails;
  
      return axiosError.response!.data as ProblemDetails;
    }
}