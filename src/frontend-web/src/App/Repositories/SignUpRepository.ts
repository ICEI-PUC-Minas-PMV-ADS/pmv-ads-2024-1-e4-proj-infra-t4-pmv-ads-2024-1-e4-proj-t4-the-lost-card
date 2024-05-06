import { AxiosError } from "axios";
import { ProblemDetails, ValidationProblemDetails } from "../DTOs/problemdetails";
import api from "./api";

export interface SignUpRequest {
    name:string,
    email: string;
    plainTextPassword: string;
  }
  
  export async function signUp(request: SignUpRequest) {
    try {
      const { data } = await api.post(
        "/players",
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