import { createContext, useContext } from "react";
import UserStore from "./userStore";
import InventoryStore from "./inventoryStore";
import CommonStore from "./commonStore";
import ProjectStore from "./projectStore";

interface Store {
  inventoryStore: InventoryStore;
  userStore: UserStore;
  commonStore: CommonStore;
  projectStore: ProjectStore;
}

export const store: Store = {
  inventoryStore: new InventoryStore(),
  userStore: new UserStore(),
  commonStore: new CommonStore(),
  projectStore: new ProjectStore(),
};

export const StoreContext = createContext(store);

export function useStore() {
  return useContext(StoreContext);
}
