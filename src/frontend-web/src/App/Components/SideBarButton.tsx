import { Link } from "react-router-dom";
import styled from "styled-components";

const Line = styled.hr`
  border: 1px solid white;
  width: 100%;
`;

const Container = styled.div`
  width: 100%;
  display: flex;
  align-items: center;
  flex-direction: column;
  color: white;
  font-family: "Roboto", sans-serif;
  font-size: 20px;
`;

interface Props {
  Text: string;
  href: string;
}

const SideBarButton = (prop: Props) => {
  return (
    <Link to={prop.href} style={{textDecoration: "none"}}>
      <Container>
        <Line />
        {prop.Text}
        <Line />
      </Container>
    </Link>
  );
};

export default SideBarButton;
