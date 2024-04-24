import React, { useState } from "react";
import styled from "styled-components";
import { ReactComponent as Eye } from "../Assets/Eye.svg";
import { ReactComponent as ClosedEye } from '../Assets/ClosedEye.svg';

const Field = styled.input`
  background-color: rgba(0, 0, 0, 0);
  color: white;
  border: none;
  padding: 10px;
  font-size: 16px;
  width: 290px;
  outline: none;
  ::placeholder {
    color: #777777;
  }
`;

const Container = styled.div`
  display: flex;
  height: 40px;
  border-radius: 1px;
  border-bottom: solid 1px #777777;
  width: 300px;
  height: 32px;
`;

interface InputProps extends React.InputHTMLAttributes<any> {}

const PasswordInput: React.FC<InputProps> = ({ children, type, ...props }) => {
  const [show, setShow] = useState(false);

  return (
    <Container>
      <Field {...props} type={show ? 'text' : 'password'}/>
      <div onClick={() => setShow(!show)}>
        {show ? <Eye /> : <ClosedEye/>}
      </div>
    </Container>
  );
};

export default PasswordInput;
