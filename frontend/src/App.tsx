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

const HomePage = () => <div>Главная</div>;
const AdminPage = () => <div>Панель администратора</div>;

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route
          path="/"
          element={
            <PrivateRoute>
              <MainLayout>{/* <HomePage /> */}</MainLayout>
            </PrivateRoute>
          }
        >
          {/* <Route path="/profile" element={<ProfilePage />} /> */}
          <Route path="/typesOfMeasure" element={<TypesOfMeasurePage />} />
          <Route path="/materials" element={<MaterialsPage />} />
          <Route path="/typesOfService" element={<TypesOfServicePage />} />
          <Route path="/contractors" element={<ContractorsPage />} />
          <Route path="/positions" element={<PositionsPage />} />
          <Route path="/workers" element={<WorkersPage />} />
          <Route path="/typesOfRepair" element={<TypesOfRepairPage />} />
          <Route path="/repairs" element={<RepairsPage />} />
        </Route>
        <Route
          path="/admin"
          element={
            <RoleRoute role="Admin">
              <MainLayout>{/* <AdminPage /> */}</MainLayout>
            </RoleRoute>
          }
        />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
