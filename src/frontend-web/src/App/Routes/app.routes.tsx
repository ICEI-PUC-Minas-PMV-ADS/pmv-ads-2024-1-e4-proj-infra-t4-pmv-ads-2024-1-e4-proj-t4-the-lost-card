import { Navigate, Route, Routes } from "react-router-dom";
import Progresso from "../Pages/Progresso";

const AppRoutes = () => {
  return (
    <Routes>
      <Route path="/" element={<Navigate replace to="/Progresso" />} />
      <Route path="/Progresso" element={<Progresso />} />
    </Routes>
  );
};

export default AppRoutes;
