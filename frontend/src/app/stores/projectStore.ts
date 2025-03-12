import { makeAutoObservable, runInAction } from "mobx";
import agent from "../agent";
import { ProjectItem } from "../../models/projectItem";

export default class ProjectStore {
  projects: ProjectItem[] = [];
  filteredProjects: ProjectItem[] = [];
  editMode = false;
  loading = false;
  loadingInitial = false;
  showFinished = false;

  constructor() {
    makeAutoObservable(this);
  }

  loadProjects = async () => {
    this.setLoadingInitial(true);
    this.projects = [];
    try {
      const loadedProjects = await agent.Projects.list();
      runInAction(() => {
        this.projects = loadedProjects;
        this.filteredProjects = this.showFinished
          ? this.projects
          : this.projects.filter((item) => !item.isFinished);
        this.setLoadingInitial(false);
      });
    } catch (error) {
      console.error(error);
      this.setLoadingInitial(false);
    }
  };

  setShowFinished = (show: boolean) => {
    this.showFinished = show;
    console.log(this.showFinished);

    runInAction(() => {
      this.filteredProjects = this.showFinished
        ? this.projects
        : this.projects.filter((item) => !item.isFinished);
    });
  };

  setLoadingInitial = (state: boolean) => {
    this.loadingInitial = state;
  };

  setLoading = (state: boolean) => {
    this.loading = state;
  };

  addOrUpdateProject = async (project: ProjectItem, file: File | undefined) => {
    this.loading = true;
    try {
      console.log(file?.name);
      this.editMode
        ? await agent.Projects.update(project)
        : await agent.Projects.create(
            file!,
            project.name,
            project.category,
            project.description
          );
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
