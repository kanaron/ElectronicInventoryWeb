import { createBrowserRouter, RouteObject } from "react-router-dom";
import App from "../../App";
import HomePage from "../../Pages/HomePage/HomePage";
import InventoryPage from "../../Pages/Inventory/InventoryPage";
import LoginForm from "../../Pages/Account/LoginForm";
import RegisterForm from "../../Pages/Account/RegisterForm";

export const routes: RouteObject[] = [
  {
    path: "/",
    element: <App />,
    children: [
      { path: "", element: <HomePage /> },
      { path: "inventory", element: <InventoryPage /> },
      { path: "login", element: <LoginForm /> },
      { path: "register", element: <RegisterForm /> },
    ],
  },
];

export const router = createBrowserRouter(routes);
