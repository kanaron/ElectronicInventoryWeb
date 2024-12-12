import React from "react";
import { Button, Container, Menu } from "semantic-ui-react";

interface NavBarProps {
  loggedIn: boolean;
  username: string;
}

const NavBar: React.FC<NavBarProps> = ({ loggedIn, username }) => {
  return (
    <Menu inverted fixed="top">
      <Container>
        <Menu.Item header>
          {/* <img src="/assets/logo.png" alt="logo" /> */}
          Electronics Inventory
        </Menu.Item>
        <Menu.Item>
          <Button positive content="Inventory" />
        </Menu.Item>
        <Menu.Menu position="right">
          <Menu.Item>
            <Button primary content="Login" />
          </Menu.Item>
          <Menu.Item>
            <Button negative content="Register" />
          </Menu.Item>
        </Menu.Menu>
      </Container>
    </Menu>
  );
};

export default NavBar;
