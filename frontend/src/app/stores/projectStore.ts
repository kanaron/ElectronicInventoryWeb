import { makeAutoObservable, runInAction } from "mobx";
import agent from "../agent";
import { ProjectItem } from "../../models/projectItem";

export default class ProjectStore {
  projects: ProjectItem[] = [];
  selectedProject: ProjectItem | undefined = undefined;
  editMode = false;
  loading = false;
  loadingInitial = false;

  constructor() {
    makeAutoObservable(this);
  }

  loadProjects = async () => {
    this.setLoadingInitial(true);
    this.projects = [];
    try {
      const loadedProjects = await agent.Projects.list();
      runInAction(() => {
        this.projects = loadedProjects.map((project) => ({
          ...project,
        }));
        this.setLoadingInitial(false);
      });
    } catch (error) {
      console.error(error);
      this.setLoadingInitial(false);
    }
  };

  setLoadingInitial = (state: boolean) => {
    this.loadingInitial = state;
  };

  setLoading = (state: boolean) => {
    this.loading = state;
  };

  selectProject = (id: string) => {
    this.selectedProject = this.projects.find((x) => x.id === id);
  };

  openForm = (id?: string) => {
    id ? this.selectProject(id) : this.cancelSelectedItem();
    id ? (this.editMode = true) : (this.editMode = false);
  };

  closeForm = () => {
    this.cancelSelectedItem();
    this.editMode = false;
  };

  cancelSelectedItem = () => {
    this.selectedProject = undefined;
  };

  addOrUpdateProject = async (project: ProjectItem) => {
    this.loading = true;
    try {
      /*this.editMode
        ? await agent.Projects.update(project)
        : await agent.Projects.create(project);*/
    } catch (error) {
      console.error(error);
    } finally {
      runInAction(() => {
        this.loading = false;
      });
    }
  };

  removeProject = async (project: ProjectItem) => {
    this.loading = true;
    try {
      await agent.Projects.delete(project.id);
      this.loadProjects();
    } catch (error) {
      console.error(error);
    } finally {
      runInAction(() => {
        this.loading = false;
      });
    }
  };
}
