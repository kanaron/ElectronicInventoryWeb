import React, { useState } from "react";
import {
  Button,
  Checkbox,
  Form,
  Header,
  Modal,
  ModalActions,
  ModalContent,
  Table,
  TableCell,
  TableHeader,
  TableRow,
} from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import { ProjectItem } from "../../models/projectItem";
import { NavLink } from "react-router-dom";

export default function ProjectsList() {
  const { projectStore, bomStore } = useStore();
  const [showFinished, setShowFinished] = useState(false);
  const [isUploadModalOpen, setIsUploadModalOpen] = useState(false);
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);
  const [selectedProject, setSelectedProject] = useState<ProjectItem>({
    id: "",
    category: "",
    description: "",
    name: "",
    isFinished: false,
  });
  const [bomFile, setBomFile] = useState<File | undefined>(undefined);

  const handleEdit = (project: ProjectItem) => {
    setSelectedProject(project);
    projectStore.editMode = true;
    setIsUploadModalOpen(true);
  };

  const handleDelete = () => {
    if (selectedProject) {
      projectStore.removeProject(selectedProject);
      setIsDeleteModalOpen(false);
    }
  };

  const handleSetFinished = () => {
    if (selectedProject) {
      selectedProject.isFinished = true;
      projectStore.editMode = true;
      projectStore.addOrUpdateProject(selectedProject, bomFile);
      setIsDeleteModalOpen(false);
    }
  };

  const openDeleteModal = (project: ProjectItem) => {
    setSelectedProject(project);
    setIsDeleteModalOpen(true);
  };

  const closeDeleteModal = () => {
    setSelectedProject({
      id: "",
      category: "",
      description: "",
      name: "",
      isFinished: false,
    });
    setIsDeleteModalOpen(false);
  };

  const openUploadModal = () => {
    projectStore.editMode = false;
    setSelectedProject({
      id: "",
      category: "",
      description: "",
      name: "",
      isFinished: false,
    });
    setIsUploadModalOpen(true);
  };

  const closeUploadModal = () => {
    setIsUploadModalOpen(false);
    setSelectedProject({
      id: "",
      category: "",
      description: "",
      name: "",
      isFinished: false,
    });
    setBomFile(undefined);
  };

  const handleShowFinished = () => {
    setShowFinished((prev) => !prev);
    projectStore.setShowFinished(showFinished);
  };

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    const { name, value } = e.target;
    setSelectedProject((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    if (event.target.files && event.target.files.length > 0) {
      setBomFile(event.target.files[0]);
    }
  };

  const handleUploadSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    await projectStore.addOrUpdateProject(selectedProject, bomFile);
    await projectStore.loadProjects();
    setIsUploadModalOpen(false);
    setBomFile(undefined);
  };

  const handleBom = (project: ProjectItem) => {
    bomStore.setSelectedProject(project);
  };

  return (
    <div>
      <div style={{ marginBottom: "10px", display: "flex", gap: "10px" }}>
        <div style={{ display: "flex", alignItems: "center", gap: "5px" }}>
          <Checkbox
            toggle
            checked={projectStore.showFinished}
            onChange={handleShowFinished}
          />
          <label style={{ fontSize: "14px", color: "#f9f9f9" }}>
            Show Finished
          </label>
          <Button
            positive
            icon="plus"
            onClick={() => openUploadModal()}
            content="Upload BOM"
            size="small"
          />
        </div>
      </div>

      <Table celled>
        <TableHeader>
          <TableRow>
            <Table.HeaderCell content="Name" />
            <Table.HeaderCell content="Category" />
            <Table.HeaderCell content="Description" />
          </TableRow>
        </TableHeader>

        <Table.Body>
          {projectStore.filteredProjects.map((project) => (
            <React.Fragment key={project.id}>
              <Table.Row>
                <TableCell content={project.name} />
                <TableCell content={project.category} />
                <TableCell content={project.description} />
                <TableCell>
                  <div style={{ display: "flex", alignItems: "center" }}>
                    <Button
                      positive
                      icon="arrow"
                      onClick={() => handleBom(project)}
                      content="Go to project"
                      size="small"
                      as={NavLink}
                      to="/bomItems"
                    />
                    <Button
                      primary
                      icon="edit"
                      onClick={() => handleEdit(project)}
                      content="Edit"
                      size="small"
                    />
                    <Button
                      icon="trash alternate"
                      color="red"
                      onClick={() => openDeleteModal(project)}
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

      <Modal open={isDeleteModalOpen} onClose={closeDeleteModal} size="tiny">
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
          <Button onClick={closeDeleteModal}>Cancel</Button>
        </Modal.Actions>
      </Modal>

      <Modal open={isUploadModalOpen} onClose={closeUploadModal} size="large">
        <Header>
          {projectStore.editMode
            ? "Edit project details"
            : "Upload BOM file and create new project"}
        </Header>
        <ModalContent>
          <Form>
            <Form.Input
              label="Project name"
              name="name"
              value={selectedProject.name}
              onChange={handleChange}
              required={true}
            />
            <Form.Input
              label="Project category"
              name="category"
              value={selectedProject.category}
              onChange={handleChange}
              required={true}
            />
            <Form.Input
              label="Project description"
              name="description"
              value={selectedProject.description}
              onChange={handleChange}
              required={false}
            />
            {!projectStore.editMode && (
              <div>
                <Form.Input
                  label="BOM file"
                  type="file"
                  required={!projectStore.editMode}
                  onChange={handleFileChange}
                />
                {bomFile && <p>Selected file: {bomFile.name}</p>}
              </div>
            )}
          </Form>
        </ModalContent>
        <ModalActions>
          <Button onClick={closeUploadModal}>Cancel</Button>
          <Button
            color="blue"
            onClick={handleUploadSubmit}
            disabled={
              !selectedProject.name ||
              !selectedProject.category ||
              (!bomFile && !projectStore.editMode)
            }
          >
            {projectStore.editMode ? "Save" : "Create"}
          </Button>
        </ModalActions>
      </Modal>
    </div>
  );
}
