import axios, { AxiosError, AxiosResponse } from "axios";
import { InventoryItem, InventoryItemCreate } from "../models/InventoryItem";
import { User, UserFormValues } from "../models/User";
import { toast } from "react-toastify";
import { store } from "./stores/store";
import { router } from "./router/Routes";
import { ServerError } from "../models/serverError";
import { ProjectItem } from "../models/projectItem";
import { BomItem } from "../models/BomItem";

axios.defaults.baseURL = process.env.REACT_APP_API_URL;

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

axios.interceptors.request.use((config) => {
  const token = store.commonStore.token;
  if (token && config.headers) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

axios.interceptors.response.use(
  async (response) => {
    return response;
  },
  (error: AxiosError) => {
    const { data, status } = error.response || {};

    if (store.userStore.isLoggedIn)
      switch (status) {
        case 400:
          if (Array.isArray(data)) {
            return Promise.reject(data);
          } else if (typeof data === "string") {
            toast.error(data || "Bad Request");
            return Promise.reject([data]);
          }
          break;
        case 401:
          toast.error(typeof data === "string" ? data : "Unauthorized");
          break;
        case 403:
          toast.error(typeof data === "string" ? data : "Forbidden");
          break;
        case 404:
          toast.error(typeof data === "string" ? data : "Not Found");
          break;
        case 500:
          const serverError = data as ServerError;
          store.commonStore.setServerError(
            serverError || "An unexpected server error occurred"
          );
          router.navigate("/server-error");
          break;
        default:
          toast.error("An unexpected error occurred");
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
  create: (inventoryItem: InventoryItemCreate) =>
    axios.post(`/Inventory/AddInventoryItem`, inventoryItem),
  update: (inventoryItem: InventoryItem) =>
    axios.put(
      `/Inventory/UpdateInventoryItem/${inventoryItem.id}`,
      inventoryItem
    ),
  delete: (id: string) => axios.delete(`/Inventory/DeleteInventoryItem/${id}`),
  fetchFromTme: (symbol: string) =>
    axios.get<InventoryItem>(`/Inventory/FetchFromTme?symbol=${symbol}`),
  fetchFromTmeQrCode: (qrCode: string) =>
    axios.get<InventoryItem>(`/Inventory/FetchFromTmeQrCode?qrCode=${qrCode}`),
};

const Projects = {
  list: () => requests.get<ProjectItem[]>("/Project/GetProjects"),
  create: (
    file: File,
    projectName: string,
    category: string,
    description?: string
  ) => {
    const formData = new FormData();
    formData.append("file", file);
    formData.append("projectName", projectName);
    formData.append("category", category);
    if (description) {
      formData.append("description", description);
    }

    return axios
      .post(`/Bom/UploadBomFile`, formData, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then(responseBody);
  },
  update: (project: ProjectItem) =>
    axios.put(`/Project/UpdateProject/${project.id}`, project),
  delete: (id: string) => axios.delete(`/Project/DeleteProject/${id}`),
};

const BomItems = {
  list: (id: string) => axios.get<BomItem[]>(`/Bom/GetBomItems/${id}`),
  update: (items: BomItem[]) => axios.put(`/Bom/UpdateBomItems`, items),
};

const Dashboard = {
  getMissingParts: () => requests.get<BomItem[]>("/Dashboard/MissingParts"),
  getLowStockItems: () => requests.get<InventoryItem[]>("/Dashboard/LowStock"),
};

const Account = {
  current: () => requests.get<User>("/Account"),
  login: (user: UserFormValues) => requests.post<User>("/Account/login", user),
  register: (user: UserFormValues) =>
    requests.post<User>("/Account/Register", user),
  updateTmeToken: (user: User) => axios.put("/Account/UpdateTmeToken", user),
};

const agent = {
  InventoryItems,
  Account,
  Projects,
  BomItems,
  Dashboard,
};

export default agent;
