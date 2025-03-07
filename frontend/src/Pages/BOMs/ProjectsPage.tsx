import React, { useEffect } from "react";
import { Container, Segment } from "semantic-ui-react";
import LoadingComponent from "../../mainComponents/LoadingComponent";
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";
import ProjectsList from "./ProjectsList";

const ProjectsPage: React.FC = () => {
  const { projectStore } = useStore();

  useEffect(() => {
    projectStore.loadProjects();
  }, [projectStore]);

  if (projectStore.loadingInitial)
    return <LoadingComponent content="Loading data" />;

  return (
    <Container fluid style={{ marginTop: "7em", padding: "0 2em" }}>
      <ProjectsList />
    </Container>
  );
};

export default observer(ProjectsPage);
