import React from 'react';
import './App.css';
import {Routes, Route, BrowserRouter} from "react-router-dom";
import Login from "../auth/Login";
import {useFetchUser} from "../hooks/UserHooks";
import Header from './Header';
import CustomersList from "../customers/CustomersList";

function App() {
     const {isSuccess} = useFetchUser()
  return (
   <BrowserRouter>
   <div className="container">
       {!isSuccess && <Login/>}
       <Header subtitle="CRM"></Header>
         <Routes>
             <Route path="/" element={<Login/>}/>
             <Route path="/customers" element={<CustomersList/>}/>
         </Routes>
   </div>
    </BrowserRouter>
  );
}

export default App;
