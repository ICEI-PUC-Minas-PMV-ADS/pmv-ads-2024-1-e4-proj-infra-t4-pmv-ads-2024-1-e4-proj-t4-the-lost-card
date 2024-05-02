import styled from "styled-components";
import { ReactComponent as Logo } from "../Assets/logo.svg";
import React from "react";
import useAuth from "../Contexts/Auth";
import SideBarButton from "./SideBarButton";
import { Link, useNavigate } from "react-router-dom";
import { ReactComponent as Logout } from "../Assets/Shutdown.svg";

const Container = styled.div`
  height: 100vh;
  width: 300px;
  display: flex;
  align-items: center;
  flex-direction: column;
  justify-content: space-around;
  padding: 0 10px;
  background-color: #5e5c72;
  font-family: "Roboto", sans-serif;
  color: white;
  font-size: 30px;
`;

const SideBar: React.FC<React.PropsWithChildren> = ({ children }) => {
  const { user, signOut } = useAuth();
  const navigate = useNavigate();

  return (
    <div style={{ display: "flex" }}>
      <Container>
        {user ? (
          <>
            <div
              style={{
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
                gap: "10px",
              }}
            >
              {user.name}
              <div onClick={() => { signOut(); navigate('/') }} style={{ cursor: 'pointer', height: '32px' }}>
                <Logout />
              </div>
            </div>
            <div style={{ width: "100%" }}>
              <SideBarButton href="Progressos" Text={"Progressos"} />
              <SideBarButton href="Cartas" Text={"Cartas"} />
            </div>
          </>
        ) : null}
        <Link to={"/"}>
          <Logo />
        </Link>
      </Container>
      {children}
    </div>
  );
};

export default SideBar;
