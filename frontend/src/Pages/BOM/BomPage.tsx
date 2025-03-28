import React, { useEffect } from "react";
import { Container } from "semantic-ui-react";
import LoadingComponent from "../../mainComponents/LoadingComponent";
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";
import BomList from "./BomList";

const BomPage: React.FC = () => {
  const { bomStore } = useStore();

  useEffect(() => {
    bomStore.loadBomItems();
  }, [bomStore]);

  if (bomStore.loadingInitial)
    return <LoadingComponent content="Loading data" />;

  return (
    <Container fluid style={{ marginTop: "7em", padding: "0 2em" }}>
      <BomList />
    </Container>
  );
};

export default observer(BomPage);
