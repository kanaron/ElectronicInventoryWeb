import { createContext, useContext } from "react";
import UserStore from "./userStore";
import InventoryStore from "./inventoryStore";
import CommonStore from "./commonStore";

interface Store {
  inventoryStore: InventoryStore;
  userStore: UserStore;
  commonStore: CommonStore;
}

export const store: Store = {
  inventoryStore: new InventoryStore(),
  userStore: new UserStore(),
  commonStore: new CommonStore(),
};

export const StoreContext = createContext(store);

export function useStore() {
  return useContext(StoreContext);
}
