import React from "react";
import ReactDOM from "react-dom/client";
import LostCardsRouters from "./App/Routes/indext.routes";
import "./index.css";
import { AuthProvider } from "./App/Contexts/Auth";

const root = ReactDOM.createRoot(
  document.getElementById("root") as HTMLElement
);
root.render(
  <React.StrictMode>
    <AuthProvider>
      <LostCardsRouters />
    </AuthProvider>
  </React.StrictMode>
);
