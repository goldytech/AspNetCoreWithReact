import React from 'react';
import './App.css';
import Header from "./Header";
import CustomersList from "../customers/CustomersList";
import useFetchUser from "../hooks/UserHooks";

function App() {
    const {isSuccess} = useFetchUser()
  return (
   <div className="container">
       {!isSuccess && <a href="/account/login">Login</a>}
     <Header subtitle="Welcome to EShop" />
       <CustomersList></CustomersList>
   </div>
  );
}

export default App;
