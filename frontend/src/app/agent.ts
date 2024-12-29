import axios, { AxiosError, AxiosResponse } from "axios";
import { InventoryItem } from "../models/InventoryItem";
import { User, UserFormValues } from "../models/User";
import { toast } from "react-toastify";
import { store } from "./stores/store";
import { router } from "./router/Routes";

axios.defaults.baseURL = "https://localhost:7000/api";

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

axios.interceptors.response.use(
  async (response) => {
    return response;
  },
  (error: AxiosError) => {
    const { data, status } = error.response as AxiosResponse;

    switch (status) {
      case 400:
        toast.error(data);
        break;
      case 401:
        toast.error(data);
        break;
      case 403:
        toast.error(data);
        break;
      case 404:
        toast.error(data);
        break;
      case 500:
        store.commonStore.setServerError(data);
        router.navigate("/server-error");
        break;
      default:
        toast.error(data);
        break;
    }
    return Promise.reject(error);
  }
);

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
  fetchFromTme: (symbol: string) =>
    axios.get<InventoryItem>(`/Inventory/FetchFromTme?symbol=${symbol}`),
};

const Account = {
  current: () => requests.get<User>("/Account"),
  login: (user: UserFormValues) => requests.post<User>("/Account/login", user),
  register: (user: UserFormValues) =>
    requests.post<User>("/Account/Register", user),
};

const agent = {
  InventoryItems,
  Account,
};

export default agent;
