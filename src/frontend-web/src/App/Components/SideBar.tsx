import styled from "styled-components";
import { ReactComponent as Logo } from "../Assets/logo.svg";
import React from "react";
import useAuth from "../Contexts/Auth";
import SideBarButton from "./SideBarButton";

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
  const { user } = useAuth();

  return (
    <div style={{ display: "flex" }}>
      <Container>
        {user ? (
          <>
            {user.name}
            <div style={{ width: "100%" }}>
              <SideBarButton href="lalaus" Text={"Lalaus"} />
              <SideBarButton href="lalaus" Text={"Lalaus"} />
              <SideBarButton href="lalaus" Text={"Lalaus"} />
              <SideBarButton href="lalaus" Text={"Lalaus"} />
            </div>
          </>
        ) : null}
        <Logo />
      </Container>
      {children}
    </div>
  );
};

export default SideBar;
