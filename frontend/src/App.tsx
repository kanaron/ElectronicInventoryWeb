import { observer } from "mobx-react-lite";
import "./App.css";
import NavBar from "./mainComponents/NavBar";
import { Container } from "semantic-ui-react";
import { Outlet } from "react-router-dom";
import { ToastContainer } from "react-toastify";

function App() {
  return (
    <>
      <ToastContainer position="bottom-right" theme="colored" hideProgressBar />
      <NavBar />
      <Container style={{ marginTop: "7em" }}>
        <Outlet />
      </Container>
    </>
  );
}

export default observer(App);
