import axios, { AxiosResponse } from "axios";
import { InventoryItem } from "../models/InventoryItem";
import { User, UserFormValues } from "../models/User";

axios.defaults.baseURL = "https://localhost:7000/api";

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const requests = {
  get: <T>(url: string) => axios.get<T>(url).then(responseBody),
  post: <T>(url: string, body: {}) =>
    axios.post<T>(url, body).then(responseBody),
  put: <T>(url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
  del: <T>(url: string) => axios.delete<T>(url).then(responseBody),
};

const InventoryItems = {
  list: () => requests.get<InventoryItem[]>("/Inventory/GetInventoryItems"),
  details: (id: string) =>
    requests.get<InventoryItem>(`/Inventory/GetInventoryItem/${id}`),
  create: (inventoryItem: InventoryItem) =>
    axios.post(`/Inventory/AddInventoryItem`, inventoryItem),
  update: (inventoryItem: InventoryItem) =>
    axios.post(
      `/Inventory/UpdateInventoryItem/${inventoryItem.id}`,
      inventoryItem
    ),
  delete: (id: string) => axios.delete(`/Inventory/DeleteInventoryItem/${id}`),
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
