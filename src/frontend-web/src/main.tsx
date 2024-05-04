import React from 'react'
import ReactDOM from 'react-dom/client'
import LostCardsRouters from "./App/Routes/indext.routes";
import './index.css'

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <LostCardsRouters />
  </React.StrictMode>,
)
