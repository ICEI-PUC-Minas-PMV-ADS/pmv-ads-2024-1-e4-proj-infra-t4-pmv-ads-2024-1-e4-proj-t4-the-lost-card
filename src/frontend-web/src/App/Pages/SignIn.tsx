import { useState } from "react";
import useAuth from "../Contexts/Auth";

const Signin : React.FC = () => {
    const { signIn } = useAuth();

    const handlerSubmit = async () => {
        await signIn({ email, plainTextPassword });
    }

    const [email, setEmail] = useState('');
    const [plainTextPassword, setPlainTextPassword] = useState('');

    return(
        <>
            <input type="text" placeholder='Email' onChange={(e) => setEmail(e.target.value)} value={email}/>
            <input type="password" placeholder='Password' onChange={(e) => setPlainTextPassword(e.target.value)} value={plainTextPassword}/>
            <button onClick={handlerSubmit}>Entrar</button>
        </>
    );
}

export default Signin;