import React, { useState } from "react";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import NavBar from "../../components/NavBar/NavBar";
import SideNav from "../../components/SideNav/SideNav";
import RegisterForm from "../RegisterForm/RegisterForm";
import LoginForm from "../LoginForm/LoginForm";
import InventoryTable from "../InventoryTable/InventoryTable";
import AddItemForm from "../AddItemForm/AddItemForm";

interface Props {}

const HomePage = (props: Props) => {
  const [loggedIn, setLoggedIn] = useState(false);
  const [username, setUsername] = useState("User");

  return (
    <Router>
      <div className="app-container">
        <NavBar loggedIn={loggedIn} username={username} />
        <div className="main-layout">
          <SideNav />
          <div className="content">
            <Routes>
              <Route path="/register" element={<RegisterForm />} />
              <Route path="/login" element={<LoginForm />} />
              <Route path="/inventory" element={<InventoryTable />} />
              <Route path="/addItem" element={<AddItemForm />} />
              {/* Add more routes as needed */}
            </Routes>
          </div>
        </div>
      </div>
    </Router>
  );
};

export default HomePage;
