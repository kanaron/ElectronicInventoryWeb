import { createBrowserRouter, RouteObject } from "react-router-dom";
import App from "../../App";
import HomePage from "../../Pages/HomePage/HomePage";
import InventoryPage from "../../Pages/Inventory/InventoryPage";
import LoginForm from "../../Pages/Account/LoginForm";
import RegisterForm from "../../Pages/Account/RegisterForm";
import ItemDetailsCard from "../../Pages/Inventory/ItemDetailsCard";
import ServerErrors from "../../Pages/errors/ServerErrors";
import ProjectsPage from "../../Pages/BOMs/ProjectsPage";
import UserSettingsPage from "../../Pages/User/UserSettingsPage";

export const routes: RouteObject[] = [
  {
    path: "/",
    element: <App />,
    children: [
      { path: "", element: <HomePage /> },
      { path: "inventory", element: <InventoryPage /> },
      { path: "addItem", element: <ItemDetailsCard /> },
      { path: "project", element: <ProjectsPage /> },
      { path: "userSettings", element: <UserSettingsPage /> },
      { path: "login", element: <LoginForm /> },
      { path: "register", element: <RegisterForm /> },
      { path: "server-error", element: <ServerErrors /> },
    ],
  },
];

export const router = createBrowserRouter(routes);
