import React from "react";
import ReactDOM from "react-dom/client";
import LostCardsRouters from "./App/Routes/indext.routes";

const root = ReactDOM.createRoot(
  document.getElementById("root") as HTMLElement
);
root.render(
  <React.StrictMode>
    <LostCardsRouters />
  </React.StrictMode>
);
