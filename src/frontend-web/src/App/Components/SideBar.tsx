import styled from "styled-components";
import { ReactComponent as Logo } from "../Assets/logo.svg";
import React from "react";

const Container = styled.div`
  height: 100vh;
  width: 300px;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 0 10px;
  background-color: #5e5c72;
`;

const SideBar: React.FC<React.PropsWithChildren> = ({ children }) => {
  return (
    <div style={{ display: 'flex' }}>
      <Container>
        <Logo />
      </Container>
      {children}
    </div>
  );
};

export default SideBar;
