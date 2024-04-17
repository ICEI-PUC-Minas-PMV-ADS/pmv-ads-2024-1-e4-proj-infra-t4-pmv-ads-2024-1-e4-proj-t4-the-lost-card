import { BrowserRouter } from "react-router-dom";
import AnonRoutes from "./anon.routes";
import useAuth, { AuthProvider } from "../Contexts/Auth";
import AppRoutes from "./app.routes";

const LostCardsRouters = () => {
  const authContext = useAuth();

  return (
    <BrowserRouter>
      <AuthProvider>
        {authContext.signed ? <AppRoutes /> : <AnonRoutes />}
      </AuthProvider>
    </BrowserRouter>
  );
};

export default LostCardsRouters;
