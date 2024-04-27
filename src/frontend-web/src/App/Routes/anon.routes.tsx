import { Navigate, Route, Routes } from "react-router-dom";
import Signin from "../Pages/SignIn";
import SideBar from "../Components/SideBar";

const AnonRoutes = () => {
  return (
    <SideBar>
      <Routes>
        <Route path="/" element={<Navigate replace to="/login" />} />
        <Route path="/login" element={<Signin />} />
      </Routes>
    </SideBar>
  );
};

export default AnonRoutes;
