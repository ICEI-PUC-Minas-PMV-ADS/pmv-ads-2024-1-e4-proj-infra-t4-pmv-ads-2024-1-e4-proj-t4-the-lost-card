import { Navigate, Route, Routes } from "react-router-dom";
import Signin from "../Pages/SignIn";
import SideBar from "../Components/SideBar";
import Signup from "../Pages/SignUp";

const AnonRoutes = () => {
  return (
    <SideBar>
      <Routes>
        <Route path="/" element={<Navigate replace to="/login" />} />
        <Route path="/login" element={<Signin />} />
        <Route path="/registro" element={<Signup />} />
      </Routes>
    </SideBar>
  );
};

export default AnonRoutes;
