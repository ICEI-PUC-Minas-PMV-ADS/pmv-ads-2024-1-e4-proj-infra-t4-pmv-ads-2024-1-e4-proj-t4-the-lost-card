import styled from "styled-components";
import PageTitle from "../Components/PageTitle";
import { CardResponse, queryCards } from "../Repositories/GameObjectRepository";
import Card from "../Components/Card";
import { useEffect, useState } from "react";

const Grid = styled.div`
  display: grid;
  grid-template-columns: 200px 200px 200px;
  gap: 50px;
  padding: 30px 100px;
`;

const Cartas = () => {
  const [cards, setCards] = useState<CardResponse>();

  useEffect(() => {
    queryCards().then((c) =>
      setCards(c)
    );
  }, []);

  return (
    <div style={{ width: "100%", gap: "30px" }}>
      <PageTitle Text={"Cartas"} onSearchClick={(v) => console.log(v)} />
      <Grid>
        {cards?.$values.map((c) => (
          <Card key={c.Id} Name={c.Name} Description={c.Description} />
        ))}
      </Grid>
    </div>
  );
};

export default Cartas;
