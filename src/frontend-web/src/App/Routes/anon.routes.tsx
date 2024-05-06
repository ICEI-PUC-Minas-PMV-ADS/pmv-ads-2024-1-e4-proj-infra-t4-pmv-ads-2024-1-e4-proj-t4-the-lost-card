import { Navigate, Route, Routes } from "react-router-dom";
import Signin from "../Pages/SignIn";
import Signup from "../Pages/SignUp";

const AnonRoutes = () => {
  return (
      <Routes>
        <Route path="/" element={<Navigate replace to="/login" />} />
        <Route path="/login" element={<Signin />} />
        <Route path="/registro" element={<Signup />} />
      </Routes>
  );
};

export default AnonRoutes;
