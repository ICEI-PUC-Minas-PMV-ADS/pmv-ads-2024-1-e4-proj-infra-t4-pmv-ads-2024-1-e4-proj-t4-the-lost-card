import { useState } from "react";
import Button from "../Components/Button";
import Input from "../Components/Input";
import {signUp} from "../Repositories/SignUpRepository"

import { ReactComponent as Account } from "../Assets/Account.svg";
import { ReactComponent as Email } from "../Assets/Email.svg";
import PasswordInput from "../Components/PasswordInput";
import styled from "styled-components";
import { useNavigate } from "react-router-dom";

const Container = styled.div`
  display: flex;
  flex-direction: column;
  justify-content: center;
  gap: 40px;
  align-items: center;
  width: 100%;
`;

const Signup: React.FC = () => {

    const [name, setName] = useState('');
    const [email, setEmail] = useState('');
    const [plainTextPassword, setPlainTextPassword] = useState('');
    const [confirmPlainTextPassword, setConfirmPlainTextPassword] = useState('');

    const navigate = useNavigate();

    async function handleSignUp() {
        if (confirmPlainTextPassword !== plainTextPassword) {
            console.log('Senhas não coincidem');
            return;
        }
       await signUp({ name, email, plainTextPassword });

       navigate('/');
    }

    return (
        <Container>
            <Input
                placeholder="Usuário"
                value={name}
                onChange={(e) => setName(e.target.value)}
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
                value={plainTextPassword}
                onChange={(e) => setPlainTextPassword(e.target.value)}
            />
            <PasswordInput
                placeholder="Confirmar senha"
                value={confirmPlainTextPassword}
                onChange={(e) => setConfirmPlainTextPassword(e.target.value)}
            />
                <Button onClick={handleSignUp}>Registrar</Button>
        </Container>
    );
};

export default Signup;