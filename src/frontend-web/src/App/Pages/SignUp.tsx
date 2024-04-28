import { useState } from "react";
import Button from "../Components/Button";
import Input from "../Components/Input";
import {signUp} from "../Repositories/SignUpRepository"

import { ReactComponent as Account } from "../Assets/Account.svg";
import { ReactComponent as Email } from "../Assets/Email.svg";
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

const Signup: React.FC = () => {

    const [username, setUsername] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');

    async function handleSignUp() {
        if (confirmPassword !== password) {
            console.log('Senhas não coincidem');
            return;
        }
       await signUp({ username, email, password });
    }

    return (
        <Container>
            <Input
                placeholder="Usuário"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
            ><Account />
            </Input>

            <Input
                placeholder="Email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
            >
                <Email />
            </Input>

            <PasswordInput
                placeholder="Senha"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
            />
            <PasswordInput
                placeholder="Confirmar senha"
                value={confirmPassword}
                onChange={(e) => setConfirmPassword(e.target.value)}
            />
                <Button onClick={handleSignUp}>Registrar</Button>
        </Container>
    );
};

export default Signup;