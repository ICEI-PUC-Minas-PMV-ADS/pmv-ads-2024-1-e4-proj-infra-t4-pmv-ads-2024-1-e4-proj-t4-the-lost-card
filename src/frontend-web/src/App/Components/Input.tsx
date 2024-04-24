import React from "react";
import styled from "styled-components";

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

interface InputProps extends React.InputHTMLAttributes<any> {

}

const Input: React.FC<InputProps> = ({children, ...props}) => {
  return (
    <Container>
      <Field {...props}/>
      {children}
    </Container>
  );
};

export default Input;
