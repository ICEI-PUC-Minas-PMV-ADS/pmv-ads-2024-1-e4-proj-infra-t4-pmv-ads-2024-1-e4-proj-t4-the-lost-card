import styled from "styled-components";
import defaultImage from "../Assets/default.jpg";

const Container = styled.div<{ hasUnlocked: boolean }>`
  width: 800px;
  height: 100px;
  display: flex;
  gap: 20px;
  align-items: center;
  background-color: ${(props) => (props.hasUnlocked ? "#5e5c72" : "#302f3d")};
  color: white;
  padding: 0 50px;
`;

const AchievmentImage = styled.img<{ hasUnlocked: boolean }>`
  ${props => props.hasUnlocked ? null : 'filter: grayscale(100%)'};
  border-radius: 100%;
`;

interface AchievmentCardProps {
  hasUnlocked: boolean;
  Description: string;
  Title: string;
}

const AchievmentsCard = ({
  hasUnlocked,
  Description,
  Title,
}: AchievmentCardProps) => {
  return (
    <Container hasUnlocked={hasUnlocked}>
      <AchievmentImage
        src={defaultImage}
        alt="defaultImage"
        hasUnlocked={hasUnlocked}
      />
      <div>
        <h3 style={{ margin: '2px' }}>{Title}</h3>
        <div>{Description}</div>
      </div>
    </Container>
  );
};

export default AchievmentsCard;
