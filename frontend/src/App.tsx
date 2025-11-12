import { BrowserRouter, Routes, Route } from "react-router-dom";
import LoginPage from "./pages/LoginPage/LoginPage";
import RegisterPage from "./pages/RegisterPage/RegisterPage";
import PrivateRoute from "./components/PrivateRoute";
import RoleRoute from "./components/RoleRoute";
import MainLayout from "./components/MainLayout";
import MaterialsPage from "./pages/MaterialsPage/MaterialsPage";
import ContractorsPage from "./pages/ContractorsPage/ContractorsPage";
import PositionsPage from "./pages/PositionsPage/PositionsPage";
import WorkersPage from "./pages/WorkersPage/WorkersPage";
import TypesOfMeasurePage from "./pages/TypesOfMeasurePage/TypesOfMeasurePage";
import TypesOfServicePage from "./pages/TypesOfServicePage/TypesOfServicePage";
import TypesOfRepairPage from "./pages/TypesOfRepairPage/TypesOfRepairPage";
import RepairsPage from "./pages/RepairsPage/RepairsPage";
import ReportsPage from "./pages/ReportsPage/ReportsPage";
import AdminUsersPage from "./pages/AdminUsersPage/AdminUsersPage";
import { useSelector } from "react-redux";
import type { RootState } from "./store";
import { jwtDecode } from "jwt-decode";
import BlockedPage from "./pages/BlockPage/BlockPage";
import ProfilePage from "./pages/ProfilePage/ProfilePage";

const HomePage = () => <div>Главная</div>;
const AdminPage = () => <div>Панель администратора</div>;

function App() {
  // const token = localStorage.getItem("token");
  // let userRole: string;
  // if (token != null) {
  //   const decoded: any = jwtDecode(token);
  //   userRole = decoded?.role;
  // }
  const userRole = useSelector((state: RootState) => state.auth.role);
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/blocked" element={<BlockedPage />} />
        <Route
          path="/"
          element={
            <PrivateRoute>
              <MainLayout role={userRole!}>{/* <HomePage /> */}</MainLayout>
            </PrivateRoute>
          }
        >
          <Route path="/profile" element={<ProfilePage />} />
          <Route path="/typesOfMeasure" element={<TypesOfMeasurePage />} />
          <Route path="/materials" element={<MaterialsPage />} />
          <Route path="/typesOfService" element={<TypesOfServicePage />} />
          <Route path="/contractors" element={<ContractorsPage />} />
          <Route path="/positions" element={<PositionsPage />} />
          <Route path="/workers" element={<WorkersPage />} />
          <Route path="/typesOfRepair" element={<TypesOfRepairPage />} />
          <Route path="/repairs" element={<RepairsPage />} />
          <Route path="/reports" element={<ReportsPage />} />
          <Route
            path="/adminPanel"
            element={
              <RoleRoute role="Admin">
                <AdminUsersPage />
              </RoleRoute>
            }
          />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;
