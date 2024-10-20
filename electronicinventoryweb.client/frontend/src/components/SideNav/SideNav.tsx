import React from "react";
import { Link } from "react-router-dom";

const SideNav: React.FC = () => {
  return (
    <div className="side-nav">
      <Link to="/inventory" className="side-nav-btn">
        Inventory
      </Link>
      <Link to="/bom" className="side-nav-btn">
        BOM
      </Link>
      <Link to="/settings" className="side-nav-btn">
        Settings
      </Link>
      {/* Add more buttons as needed */}
    </div>
  );
};

export default SideNav;
