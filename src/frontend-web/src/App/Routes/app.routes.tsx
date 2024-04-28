import { Navigate, Route, Routes } from "react-router-dom";
import Bestiario from "../Pages/Bestiario";

const AppRoutes = () => {
  return (
    <Routes>
      <Route path="/" element={<Navigate replace to="/bestiario" />} />
      <Route path="/bestiario" element={<Bestiario />} />
    </Routes>
  );
};

export default AppRoutes;
