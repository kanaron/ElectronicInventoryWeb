import { createContext, useContext } from "react";
import UserStore from "./userStore";
import InventoryStore from "./inventoryStore";

interface Store {
  inventoryStore: InventoryStore;
  userStore: UserStore;
}

export const store: Store = {
  inventoryStore: new InventoryStore(),
  userStore: new UserStore(),
};

export const StoreContext = createContext(store);

export function useStore() {
  return useContext(StoreContext);
}
