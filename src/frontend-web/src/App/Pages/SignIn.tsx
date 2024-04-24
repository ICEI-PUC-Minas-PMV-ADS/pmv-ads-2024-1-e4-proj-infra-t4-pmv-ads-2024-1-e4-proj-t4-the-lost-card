import { useState } from "react";
import useAuth from "../Contexts/Auth";
import Button from "../Components/Button";
import Input from "../Components/Input";
import { ReactComponent as Logo } from "../Assets/logo.svg";
import PasswordInput from "../Components/PasswordInput";

const Signin: React.FC = () => {
  const { signIn } = useAuth();

  const handlerSubmit = async () => {
    await signIn({ email, plainTextPassword });
  };

  const [email, setEmail] = useState("");
  const [plainTextPassword, setPlainTextPassword] = useState("");

  return (
    <div style={{ backgroundColor: "black" }}>
      <Input
        type="text"
        placeholder="Email"
        onChange={(e) => setEmail(e.target.value)}
        value={email}
      >
        <Logo />
      </Input>
      <PasswordInput
        placeholder="Password"
        onChange={(e) => setPlainTextPassword(e.target.value)}
        value={plainTextPassword}
      />
      <Button onClick={handlerSubmit}>Entrar</Button>
    </div>
  );
};

export default Signin;
