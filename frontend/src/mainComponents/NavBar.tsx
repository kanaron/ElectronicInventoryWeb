import { NavLink } from "react-router-dom";
import { Button, Container, Dropdown, Menu } from "semantic-ui-react";
import { useStore } from "../app/stores/store";
import { observer } from "mobx-react-lite";

export default observer(function NavBar() {
  const { userStore } = useStore();

  const handleLogout = () => {
    userStore.logout();
  };

  return (
    <Menu inverted fixed="top">
      <Container>
        <Menu.Item header as={NavLink} to="/">
          {/* <img src="/assets/logo.png" alt="logo" /> */}
          Electronics Inventory
        </Menu.Item>
        <Menu.Item>
          <Button
            positive
            icon="database"
            content="Inventory"
            as={NavLink}
            to="/inventory"
          />
        </Menu.Item>
        <Menu.Item>
          <Button
            positive
            icon="database"
            content="Projects"
            as={NavLink}
            to="/project"
          />
        </Menu.Item>

        <Menu.Menu position="right">
          <Menu.Item>
            <Dropdown pointing="top left" text={userStore.user?.userName}>
              <Dropdown.Menu>
                <Dropdown.Item
                  onClick={handleLogout}
                  text="Logout"
                  icon="power"
                />
              </Dropdown.Menu>
            </Dropdown>
          </Menu.Item>
        </Menu.Menu>
      </Container>
    </Menu>
  );
});
