import { Navigate, Route, Routes } from "react-router-dom";
import Signin from "../Pages/SignIn";

const AnonRoutes = () => {
  return (
    <Routes>
      <Route path="/" element={<Navigate replace to="/login" />} />
      <Route path="/login" element={<Signin />} />
    </Routes>
  );
};

export default AnonRoutes;
