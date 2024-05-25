import React, { ReactElement, createContext, useState } from 'react';
import * as SignInRepository from "../Repositories/SignInRepository";
import { ProblemDetails } from "../DTOs/problemdetails";

const AuthContext = createContext<AuthContextData>({} as AuthContextData);

interface AuthContextData {
  user: User | null;
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

    if ("title" in response) {
      return response;
    }

    setUser(response);
  }

  function signOut() {
    setUser(null);
  }

  return (
    <AuthContext.Provider value={{ user, signIn, signOut }}>
      {children}
    </AuthContext.Provider>
  );
};

export default AuthContext;
