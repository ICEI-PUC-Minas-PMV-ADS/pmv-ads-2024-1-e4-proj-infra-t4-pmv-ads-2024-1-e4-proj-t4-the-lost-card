import { Navigate, Route, Routes } from "react-router-dom";
import Progresso from "../Pages/Progresso";
import Cartas from "../Pages/Cartas";

const AppRoutes = () => {
  return (
    <Routes>
      <Route path="/" element={<Navigate replace to="/Progresso" />} />
      <Route path="/Progresso" element={<Progresso />} />
      <Route path="/Cartas" element={<Cartas />} />
    </Routes>
  );
};

export default AppRoutes;
