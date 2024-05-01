import { Navigate, Route, Routes } from "react-router-dom";
import Progressos from "../Pages/Progressos";

const AppRoutes = () => {
  return (
    <Routes>
      <Route path="/" element={<Navigate replace to="/Progressos" />} />
      <Route path="/Progressos" element={<Progressos />} />
    </Routes>
  );
};

export default AppRoutes;
