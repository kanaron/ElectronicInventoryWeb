import { NavLink } from "react-router-dom";
import { Button, Container, Header, Segment } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";

export default observer(function HomePage() {
  const { userStore } = useStore();

  return (
    <>
      {!userStore.isLoggedIn && (
        <Segment inverted textAlign="center" className="home-segment">
          <Container>
            <Header as="h1" inverted className="home-header">
              Electronics Inventory
            </Header>
            <Segment vertical className="button-segment">
              <Button
                primary
                size="large"
                className="home-button"
                content="Login"
                as={NavLink}
                to="/login"
              />
              <Button
                negative
                size="large"
                className="home-button"
                content="Register"
                as={NavLink}
                to="/register"
              />
            </Segment>
          </Container>
        </Segment>
      )}
    </>
  );
});
