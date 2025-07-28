import { Navigate } from "react-router-dom";
import { jwtDecode } from "jwt-decode";
import type { ReactNode } from "react";

interface Props {
  children: ReactNode;
  role: string;
}

const RoleRoute = ({ children, role }: Props) => {
  const token = localStorage.getItem("token");
  if (!token) return <Navigate to="/login" />;
  const decoded: any = jwtDecode(token);
  return decoded?.role === role ? children : <Navigate to="/" />;
};

export default RoleRoute;
