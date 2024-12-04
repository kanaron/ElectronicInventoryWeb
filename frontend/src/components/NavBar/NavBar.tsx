import React from "react";
import { Link } from "react-router-dom";

interface NavBarProps {
  loggedIn: boolean;
  username: string;
}

const NavBar: React.FC<NavBarProps> = ({ loggedIn, username }) => {
  return (
    <div className="navbar">
      <div className="navbar-right">
        {loggedIn ? (
          <div className="user-dropdown">
            <span>{username} ▼</span>
            <ul className="dropdown-content">
              <li>
                <Link to="/account">Account</Link>
              </li>
              <li>
                <Link to="/settings">Settings</Link>
              </li>
              <li>
                <Link to="/logout">Logout</Link>
              </li>
            </ul>
          </div>
        ) : (
          <div>
            <Link to="/login">Login</Link>
            <Link to="/register">Register</Link>
          </div>
        )}
      </div>
    </div>
  );
};

export default NavBar;
