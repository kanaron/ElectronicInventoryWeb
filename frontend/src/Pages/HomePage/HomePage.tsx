import React, { Fragment, useState } from "react";
import NavBar from "../../mainComponents/NavBar";
import { Container } from "semantic-ui-react";
import InventoryPage from "../Inventory/InventoryPage";

interface Props {}

const HomePage = (props: Props) => {
  const [loggedIn, setLoggedIn] = useState(false);
  const [username, setUsername] = useState("User");

  return (
    <Fragment>
      <NavBar loggedIn={loggedIn} username={username} />
      <Container style={{ marginTop: "7em" }}>
        <InventoryPage />
      </Container>
    </Fragment>
  );
};

export default HomePage;
