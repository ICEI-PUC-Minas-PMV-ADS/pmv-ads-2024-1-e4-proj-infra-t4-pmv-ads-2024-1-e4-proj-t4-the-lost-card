import React, { ReactElement, createContext, useState } from 'react';
import * as SignInRepository from "../Repositories/SignInRepository";
import { ProblemDetails } from "../DTOs/problemdetails";

const AuthContext = createContext<AuthContextData>({} as AuthContextData);

interface AuthContextData {
  signed: boolean;
  user: User | null;
  setToken(token: string): void;
  signIn(
    request: AuthRequest
  ): Promise<ProblemDetails | undefined>;
  signOut(): void;
}

interface User {
  name: string;
  token: string;
  id: string;
}

interface AuthProviderProps extends React.PropsWithChildren {
  children: ReactElement;
}

interface AuthRequest {
  email: string;
  plainTextPassword: string;
}

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);

  async function signIn(request: SignInRepository.SignInRequest) {
    const response = await SignInRepository.signIn(request);

    if ("detail" in response) {
      return response;
    }

    setUser(response);
  }

  function signOut() {
    setUser(null);
  }

  function setToken(token: string) {
    setUser(user => {
      if (user)
        user.token = token
      else
        user = {token: token, name: "adeilton", id: "4826f484-1221-4e53-916c-08dc6402df5e"}
      
      return user;
    });
  }

  return (
    <AuthContext.Provider value={{ signed: Boolean(user), user, setToken, signIn, signOut }}>
      {children}
    </AuthContext.Provider>
  );
};

export default AuthContext;
