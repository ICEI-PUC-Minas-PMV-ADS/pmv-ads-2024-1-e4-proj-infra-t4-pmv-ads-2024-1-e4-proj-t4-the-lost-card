import React, {
  PropsWithChildren,
  createContext,
  useContext,
  useEffect,
  useState,
} from "react";
import * as SignInRepository from "../Repositories/SignInRepository";
import { ProblemDetails } from "../DTOs/problemdetails";

const AuthContext = createContext<AuthContextData>({} as AuthContextData);

interface AuthContextData {
  signed: boolean;
  user: User | null;
  signIn(
    request: SignInRepository.SignInRequest
  ): Promise<ProblemDetails | undefined>;
  signOut(): void;
}

interface User {
  name: string;
  token: string;
}

export const AuthProvider: React.FC<PropsWithChildren> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);

  async function signIn(request: SignInRepository.SignInRequest) {
    const response = await SignInRepository.signIn(request);

    if ("detail" in response) {
      return response;
    }

    localStorage.setItem("token", response.token);
    localStorage.setItem("name", response.name);
    setUser(response);
  }

  function signOut() {
    setUser(null);
  }

  useEffect(() => {
    const token = localStorage.getItem("token");
    const name = localStorage.getItem("name");

    if (token && name && !Boolean(user)) {
      setUser({ token, name });
    }
  }, [user]);

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
