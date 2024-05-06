import { useState } from "react";
import useAuth from "../Contexts/Auth";
import Button from "../Components/Button";
import Input from "../Components/Input";
import Account from "../Assets/Account.svg?react";
import PasswordInput from "../Components/PasswordInput";
import styled from "styled-components";
import { Link } from "react-router-dom";
import { useNavigate } from "react-router-dom";

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
  const navigate = useNavigate();

  const handlerSubmit = async () => {
    await signIn({ email, plainTextPassword });

    navigate('/');
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
        <Link to={"/registro"}>
          <Button>Criar</Button>
        </Link>
      </div>
    </Container>
  );
};

export default Signin;
