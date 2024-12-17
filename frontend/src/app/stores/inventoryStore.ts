import { makeAutoObservable, runInAction } from "mobx";
import { InventoryItem } from "../../models/InventoryItem";
import agent from "../agent";

export default class InventoryStore {
  items: InventoryItem[] = [];
  selectedItem: InventoryItem | undefined = undefined;
  editMode = false;
  loading = false;
  loadingInitial = false;

  constructor() {
    makeAutoObservable(this);
  }

  loadItems = async () => {
    this.setLoadingInitial(true);
    this.items.length = 0;

    try {
      const loadedItems = await agent.InventoryItems.list();
      loadedItems.forEach((item) => {
        item.dateAdded = item.dateAdded.split("T")[0];
        item.lastUpdated = item.lastUpdated.split("T")[0];
        this.items.push(item);
        this.setLoadingInitial(false);
      });

      this.loadingInitial = false;
    } catch (error) {
      console.log(error);
      this.setLoadingInitial(false);
    }
  };

  setLoadingInitial = (state: boolean) => {
    this.loadingInitial = state;
  };

  setLoading = (state: boolean) => {
    this.loading = state;
  };

  selectItem = (id: number) => {
    this.selectedItem = this.items.find((x) => x.id === id);
  };

  cancelSelectedItem = () => {
    this.selectedItem = undefined;
  };

  openForm = (id?: number) => {
    id ? this.selectItem(id) : this.cancelSelectedItem();
    id ? (this.editMode = true) : (this.editMode = false);
  };

  closeForm = () => {
    this.cancelSelectedItem();
    this.editMode = false;
  };

  addOrUpdateItem = async (item: InventoryItem) => {
    this.loading = true;
    try {
      this.editMode
        ? await agent.InventoryItems.update(item)
        : await agent.InventoryItems.create(item);
      runInAction(() => {
        if (this.editMode) {
          this.items.filter((x) => x.id !== item.id);
        }
        this.items.push(item);
      });
    } catch (error) {
      console.log(error);
    } finally {
      runInAction(() => {
        this.loading = false;
      });
    }
  };
}
