import { makeAutoObservable, runInAction } from "mobx";
import agent from "../agent";
import { ProjectItem } from "../../models/projectItem";
import { BomItem } from "../../models/BomItem";

export default class BomStore {
  project: ProjectItem | undefined = undefined;
  bomItems: BomItem[] = [];
  filteredBomItems: BomItem[] = [];
  loading = false;
  loadingInitial = false;
  showIrrevelant = false;
  showPlaced = false;
  areUnsavedChanges = false;

  constructor() {
    makeAutoObservable(this);
  }

  loadBomItems = async () => {
    console.log("bom store load bom items");
    this.setLoadingInitial(true);
    this.bomItems = [];
    try {
      const loadedBomItems = (await agent.BomItems.list(this.project?.id!))
        .data;
      console.log("number of items: " + this.bomItems.length);
      //runInAction(() => {
      this.bomItems = loadedBomItems;
      this.setFilters();
      this.setLoadingInitial(false);
      //});
    } catch (error) {
      console.error(error);
      this.setLoadingInitial(false);
    }
  };

  setFilters = () => {
    this.filteredBomItems = this.showIrrevelant
      ? this.bomItems
      : this.bomItems.filter((item) => item.isRelevant);
    this.filteredBomItems = this.showPlaced
      ? this.filteredBomItems
      : this.filteredBomItems.filter((item) => !item.isPlaced);
  };

  setShowIrrelevant = (show: boolean) => {
    this.showIrrevelant = show;

    runInAction(() => {
      this.setFilters();
    });
  };

  setShowPlaced = (show: boolean) => {
    this.showPlaced = show;

    runInAction(() => {
      this.setFilters();
    });
  };

  toggleRelevance = (id: string) => {
    const item = this.bomItems.find((i) => i.id === id);
    if (item) {
      item.isRelevant = !item.isRelevant;
      this.areUnsavedChanges = true;
      this.setFilters();
    }
  };

  togglePlaced = (id: string) => {
    const item = this.bomItems.find((i) => i.id === id);
    if (item) {
      item.isPlaced = !item.isPlaced;
      this.areUnsavedChanges = true;
      this.setFilters();
    }
  };

  setLoadingInitial = (state: boolean) => {
    this.loadingInitial = state;
  };

  setLoading = (state: boolean) => {
    this.loading = state;
  };

  setSelectedProject = (selectedProject: ProjectItem | undefined) => {
    this.project = selectedProject;
    console.log("Bom store set project: " + selectedProject?.name);
  };

  updateBomItem = async (item: BomItem) => {
    this.loading = true;
    try {
      await agent.BomItems.update(item);
    } catch (error) {
      console.error(error);
    } finally {
      runInAction(() => {
        this.loading = false;
      });
    }
  };
}
