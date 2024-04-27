import { useState } from "react";
import useAuth from "../Contexts/Auth";
import Button from "../Components/Button";
import Input from "../Components/Input";
import { ReactComponent as Account } from "../Assets/Account.svg";
import PasswordInput from "../Components/PasswordInput";
import styled from "styled-components";

const Container = styled.div`
  display: flex;
  flex-direction: column;
  justify-content: center;
  gap: 40px;
  align-items: center;
  width: 100%;
`;

const Signin: React.FC = () => {
  const { signIn } = useAuth();

  const handlerSubmit = async () => {
    await signIn({ email, plainTextPassword });
  };

  const [email, setEmail] = useState("");
  const [plainTextPassword, setPlainTextPassword] = useState("");

  return (
    <Container>
      <Input
        placeholder="Email"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
      >
        <Account />
      </Input>
      <PasswordInput
        placeholder="Senha"
        value={plainTextPassword}
        onChange={(e) => setPlainTextPassword(e.target.value)}
      />
      <div
        style={{
          display: "flex",
          justifyContent: "space-between",
          width: "300px",
        }}
      >
        <Button onClick={handlerSubmit}>Entrar</Button>
        <Button>Criar</Button>
      </div>
    </Container>
  );
};

export default Signin;
