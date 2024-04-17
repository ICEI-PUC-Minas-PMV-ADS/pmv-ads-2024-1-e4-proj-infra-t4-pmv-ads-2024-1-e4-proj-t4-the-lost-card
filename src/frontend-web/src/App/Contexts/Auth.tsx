import React, {
  PropsWithChildren,
  createContext,
  useContext,
  useState,
} from "react";
import * as SignInRepository from "../Repositories/SignInRepository";
import { ProblemDetails } from "../DTOs/problemdetails";

const AuthContext = createContext<AuthContextData>({} as AuthContextData);

interface AuthContextData {
  signed: boolean;
  user: object | null;
  signIn(
    request: SignInRepository.SignInRequest
  ): Promise<SignInRepository.SignInResponse | ProblemDetails>;
  signOut(): void;
}

export const AuthProvider: React.FC<PropsWithChildren> = ({ children }) => {
  const [user, setUser] = useState<object | null>(null);

  async function signIn(request: SignInRepository.SignInRequest) {
    const response = await SignInRepository.signIn(request);

    if ("token" in response) setUser(response);

    return response;
  }

  function signOut() {
    setUser(null);
  }

  return (
    <AuthContext.Provider
      value={{ signed: Boolean(user), user, signIn, signOut }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export default function useAuth() {
  const context = useContext(AuthContext);

  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }

  return context;
}