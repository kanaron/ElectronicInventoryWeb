import axios, { AxiosResponse } from "axios";
import { InventoryItem } from "../models/InventoryItem";
import { User, UserFormValues } from "../models/User";

axios.defaults.baseURL = "https://localhost:7000/api";

const responseBody = (response: AxiosResponse) => response.data;

const request = {
  get: (url: string) => axios.get(url).then(responseBody),
  post: (url: string, body: {}) => axios.post(url, body).then(responseBody),
  put: (url: string, body: {}) => axios.put(url, body).then(responseBody),
  del: (url: string) => axios.delete(url).then(responseBody),
};

const InventoryItems = {
  list: () => axios.get<InventoryItem[]>("/Inventory/GetInventoryItems"),
  details: (id: string) =>
    axios.get<InventoryItem>(`/Inventory/GetInventoryItem/${id}`),
  create: (inventoryItem: InventoryItem) =>
    axios.post<void>(`/Inventory/AddInventoryItem`, inventoryItem),
  update: (inventoryItem: InventoryItem) =>
    axios.post<void>(
      `/Inventory/UpdateInventoryItem/${inventoryItem.id}`,
      inventoryItem
    ),
  delete: (id: string) =>
    axios.delete<void>(`/Inventory/DeleteInventoryItem/${id}`),
};

const Account = {
  current: () => axios.get<User>("/Account"),
  login: (user: UserFormValues) => axios.post<User>("/Account/Login", user),
  register: (user: UserFormValues) =>
    axios.post<User>("/Account/Register", user),
};

const agent = {
  InventoryItems,
  Account,
};

export default agent;
