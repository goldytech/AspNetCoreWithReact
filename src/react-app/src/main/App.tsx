import React from 'react';
import './App.css';
import Header from "./Header";
import CustomersList from "../customers/CustomersList";

function App() {
  return (
   <div className="container">
     <Header subtitle="Welcome to EShop" />
       <CustomersList></CustomersList>
   </div>
  );
}

export default App;
