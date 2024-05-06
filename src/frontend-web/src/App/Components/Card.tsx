import styled from "styled-components";
import image from "../Assets/BlockCardDefaultImage.png";

const Container = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
  flex-direction: column;
  width: 200px;
  height: 350px;
  background-color: #5e5c72;
  border-radius: 8px;
  color: white;
`;

const Nome = styled.div`
  width: 25px;
  font-size: 20px;
  position: relative;
  top: 10px;
  text-align: center;
  background-color: #929292;
  width: 150px;
`;

const Image = styled.img`
  width: 100%;
  background-color: wheat;
`;

export interface CardProp {
  Name: string;
  Description: string;
}

const Card = ({ Name, Description }: CardProp) => {
  return (
    <Container>
      <Nome>{Name}</Nome>
      <Image src={image} />
      <div style={{ width: "150px" }}>{Description}</div>
    </Container>
  );
};

export default Card;
