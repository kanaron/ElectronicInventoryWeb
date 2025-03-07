import React, { useState } from "react";
import {
  Button,
  Checkbox,
  Header,
  Modal,
  ModalContent,
  Table,
  TableCell,
  TableHeader,
  TableRow,
} from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import { NavLink } from "react-router-dom";
import { ProjectItem } from "../../models/projectItem";

export default function ProjectsList() {
  const { projectStore } = useStore();
  const [showFinished, setShowFinished] = useState(false);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedProject, setSelectedProject] = useState<ProjectItem | null>(
    null
  );

  const handleEdit = (selectedProject: ProjectItem) => {
    projectStore.selectedProject = selectedProject;
    projectStore.openForm(selectedProject.id);
  };

  const handleDelete = () => {
    if (selectedProject) {
      projectStore.removeProject(selectedProject);
      setIsModalOpen(false);
    }
  };

  const handleSetFinished = () => {
    if (selectedProject) {
      selectedProject.isFinished = true;
      projectStore.editMode = true;
      projectStore.addOrUpdateProject(selectedProject);
      setIsModalOpen(false);
    }
  };

  const openModal = (project: ProjectItem) => {
    setSelectedProject(project);
    setIsModalOpen(true);
  };

  const closeModal = () => {
    setSelectedProject(null);
    setIsModalOpen(false);
  };

  const handleShowFinished = () => {
    setShowFinished((prev) => !prev);
  };

  return (
    <div>
      <div style={{ marginBottom: "10px", display: "flex", gap: "10px" }}>
        <div style={{ display: "flex", alignItems: "center", gap: "5px" }}>
          <Checkbox
            toggle
            checked={showFinished}
            onChange={handleShowFinished}
          />
          <label style={{ fontSize: "14px", color: "#f9f9f9" }}>
            Show Finished
          </label>
        </div>
      </div>

      <Table celled>
        <TableHeader>
          <TableRow>
            <Table.HeaderCell content="Name" />
            <Table.HeaderCell content="Category" />
            <Table.HeaderCell content="Description" />
            <Table.HeaderCell content="IsFinished" />
          </TableRow>
        </TableHeader>

        <Table.Body>
          {projectStore.projects.map((project) => (
            <React.Fragment key={project.id}>
              <Table.Row>
                <TableCell content={project.name} />
                <TableCell content={project.category} />
                <TableCell content={project.description} />
                <TableCell content={project.isFinished} />
                <TableCell>
                  <div style={{ display: "flex", alignItems: "center" }}>
                    <Button
                      primary
                      icon="edit"
                      onClick={() => handleEdit(project)}
                      content="Edit"
                      size="small"
                      as={NavLink}
                      to="/addProject"
                    />
                    <Button
                      icon="trash alternate"
                      color="red"
                      onClick={() => openModal(project)}
                      content="Remove"
                      size="small"
                    />
                  </div>
                </TableCell>
              </Table.Row>
            </React.Fragment>
          ))}
        </Table.Body>
      </Table>

      <Modal open={isModalOpen} onClose={closeModal} size="tiny">
        <Header>Confirm Action</Header>
        <ModalContent>
          <p style={{ color: "#333" }}>
            Do you want to permanently delete this project or set it as
            finished?
          </p>
        </ModalContent>
        <Modal.Actions>
          <Button color="red" onClick={handleDelete}>
            Remove
          </Button>
          <Button color="yellow" onClick={handleSetFinished}>
            Set as Finished
          </Button>
          <Button onClick={closeModal}>Cancel</Button>
        </Modal.Actions>
      </Modal>
    </div>
  );
}
