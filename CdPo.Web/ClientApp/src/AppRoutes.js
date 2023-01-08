import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";

const AppRoutes = [
  {
    path: '/print-forms',
    element: <FetchData />
  }
];

export default AppRoutes;
