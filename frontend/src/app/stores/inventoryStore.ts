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
    this.items = [];
    try {
      const loadedItems = await agent.InventoryItems.list();
      runInAction(() => {
        this.items = loadedItems.map((item) => ({
          ...item,
          dateAdded: item.dateAdded.split("T")[0],
          lastUpdated: item.lastUpdated.split("T")[0],
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

  selectItem = (id: string) => {
    this.selectedItem = this.items.find((x) => x.id === id);
  };

  cancelSelectedItem = () => {
    this.selectedItem = undefined;
  };

  openForm = (id?: string) => {
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
    } catch (error) {
      console.error(error);
    } finally {
      runInAction(() => {
        this.loading = false;
      });
    }
  };

  removeItem = async (item: InventoryItem) => {
    this.loading = true;
    try {
      await agent.InventoryItems.delete(item.id);
      this.loadItems();
    } catch (error) {
      console.error(error);
    } finally {
      runInAction(() => {
        this.loading = false;
      });
    }
  };
}
