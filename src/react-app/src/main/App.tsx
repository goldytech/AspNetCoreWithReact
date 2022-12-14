import React from 'react';
import './App.css';
import Header from "./Header";
import CustomersList from "../customers/CustomersList";
// import useFetchUser from "../hooks/UserHooks";
import Login from "../auth/Login";

function App() {
    // const {isSuccess} = useFetchUser()
  return (
   <div className="container">
       <Login></Login>
     {/*  {!isSuccess && <a href="/account/login">Login</a>}*/}
     {/*<Header subtitle="Welcome to EShop" />*/}
     {/*  <CustomersList></CustomersList>*/}
   </div>
  );
}

export default App;
