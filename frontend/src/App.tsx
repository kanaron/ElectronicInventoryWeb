import { observer } from "mobx-react-lite";
import "./App.css";
import NavBar from "./mainComponents/NavBar";
import { Container } from "semantic-ui-react";
import { Outlet } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import { useStore } from "./app/stores/store";
import { useEffect } from "react";
import LoadingComponent from "./mainComponents/LoadingComponent";

function App() {
  const { commonStore, userStore } = useStore();

  useEffect(() => {
    if (commonStore.token) {
      userStore.getUser().finally(() => commonStore.setAppLoaded());
    } else {
      commonStore.setAppLoaded();
    }
  }, [commonStore, userStore]);

  if (!commonStore.appLoaded)
    return <LoadingComponent content="Loading app..." />;

  return (
    <>
      <ToastContainer position="bottom-right" theme="colored" hideProgressBar />
      {userStore.isLoggedIn && <NavBar />}
      <Container style={{ marginTop: "7em" }} fluid>
        <Outlet />
      </Container>
    </>
  );
}

export default observer(App);
