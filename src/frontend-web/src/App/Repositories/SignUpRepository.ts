import axios, { AxiosError } from "axios";
import { ProblemDetails, ValidationProblemDetails } from "../DTOs/problemdetails";

export interface SignUpRequest {
    username:string,
    email: string;
    password: string;
  }
  
  export async function signUp(request: SignUpRequest) {
    try {
      const { data } = await axios.post(
        "http://localhost:7097/api/players",
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