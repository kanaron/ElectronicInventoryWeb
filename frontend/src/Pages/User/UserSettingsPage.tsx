import { observer } from "mobx-react-lite";
import { useEffect, useState } from "react";
import { useStore } from "../../app/stores/store";
import { Container, Form, Header } from "semantic-ui-react";

export default observer(function UserSettingsPage() {
  const { userStore } = useStore();
  const [token, setToken] = useState("");

  useEffect(() => {
    if (userStore.user?.tmeToken) {
      setToken(userStore.user.tmeToken);
    }
  }, [userStore.user]);

  const handleSubmit = () => {
    userStore.updateTmeToken(token);
  };

  return (
    <Container style={{ marginTop: "7em" }}>
      <Header as="h2" content="User Settings" />
      <Form onSubmit={handleSubmit}>
        <Form.Input
          label="TME API Token"
          placeholder="Enter your private TME token"
          value={token}
          onChange={(e) => setToken(e.target.value)}
        />
        <Form.Button positive content="Save Changes" />
      </Form>
    </Container>
  );
});
