import { BrowserRouter } from "react-router-dom";
import AnonRoutes from "./anon.routes";
import useAuth from "../Contexts/Auth";
import AppRoutes from "./app.routes";
import SideBar from "../Components/SideBar";

const LostCardsRouters = () => {
  const { signed } = useAuth();

  return (
    <BrowserRouter>
      <SideBar>{signed ? <AppRoutes /> : <AnonRoutes />}</SideBar>
    </BrowserRouter>
  );
};

export default LostCardsRouters;
