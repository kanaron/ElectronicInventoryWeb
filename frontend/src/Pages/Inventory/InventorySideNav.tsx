import React from "react";
import { Button, Container, Menu, MenuItem } from "semantic-ui-react";

const InventorySideNav: React.FC = () => {
  return (
    <Menu inverted fixed="left" vertical style={{ top: "48px" }}>
      <Container>
        <Menu.Item>
          <Button positive content="Inventory table" />
        </Menu.Item>
        <Menu.Item>
          <Button positive content="Add Item" />
        </Menu.Item>
      </Container>
    </Menu>
  );
};

export default InventorySideNav;
