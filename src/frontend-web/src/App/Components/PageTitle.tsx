import Input from "./Input";
import SearchIcon from "../Assets/Vector.svg?react";
import { useState } from "react";
import styled from "styled-components";

interface PageTitleProp {
  Text: string;
  onSearchClick?: (value: string) => void;
}

const Container = styled.div`
  width: 100%;
  height: 50px;
  display: flex;
  justify-content: space-around;
  align-items: center;
`;

const PageTitle = ({ Text, onSearchClick }: PageTitleProp) => {
  const [search, setSearch] = useState("");

  return (
    <Container>
      <div style={{ fontSize: "30px", textAlign: "end", width: "300px", color: "white" }}>
        {Text}
      </div>
      {onSearchClick ? (
        <Input
          value={search}
          placeholder="Search"
          onChange={(e) => setSearch(e.target.value)}
        >
          <div onClick={() => onSearchClick(search)}>
            <SearchIcon />
          </div>
        </Input>
      ) : null}
    </Container>
  );
};

export default PageTitle;