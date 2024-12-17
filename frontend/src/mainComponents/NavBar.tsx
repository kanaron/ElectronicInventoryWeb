import React from "react";
import { NavLink } from "react-router-dom";
import { Button, Container, Menu } from "semantic-ui-react";

const NavBar: React.FC = () => {
  return (
    <Menu inverted fixed="top">
      <Container>
        <Menu.Item header as={NavLink} to="/">
          {/* <img src="/assets/logo.png" alt="logo" /> */}
          Electronics Inventory
        </Menu.Item>
        <Menu.Item>
          <Button positive content="Inventory" as={NavLink} to="/inventory" />
        </Menu.Item>
        <Menu.Item>
          <Button positive content="Add item" as={NavLink} to="/addItem" />
        </Menu.Item>
        <Menu.Menu position="right">
          <Menu.Item>
            <Button primary content="Login" as={NavLink} to="/login" />
          </Menu.Item>
          <Menu.Item>
            <Button negative content="Register" as={NavLink} to="/register" />
          </Menu.Item>
        </Menu.Menu>
      </Container>
    </Menu>
  );
};

export default NavBar;
