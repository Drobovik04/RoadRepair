import type { ReactNode } from "react";
import { useEffect, useState } from "react";
import { Navigate } from "react-router-dom";
import { checkLoginAndGetInfoAboutUser } from "../services/auth";
import { Spin } from "antd";
import { useDispatch, useSelector } from "react-redux";
import { setInfo } from "../store/authSlice";
import type { RootState } from "../store";

const PrivateRoute = ({ children }: { children: ReactNode }) => {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean | null>(null); // null — загрузка

  const dispatch = useDispatch();
  const isBlocked = useSelector((state: RootState) => state.auth.isBlocked);
  // хз как исправить, он кидает ошибку в любом случае если нет логина по причине вызова PrivateRoute
  useEffect(() => {
    const verify = async () => {
      checkLoginAndGetInfoAboutUser()
        .then((res) => {
          setIsAuthenticated(res.status === 200);
          dispatch(setInfo(JSON.stringify(res.data)));
        })
        .catch((err) => console.log(err));
    };
    verify();
  }, []);

  if (isAuthenticated === null) {
    return (
      <div
        style={{
          display: "flex",
          height: "100vh",
          justifyContent: "center",
          alignItems: "center",
        }}
      >
        <Spin size="large" />
      </div>
    );
  }

  if (isBlocked) {
    localStorage.removeItem("token");
    return <Navigate to="/blocked" replace />;
  }

  if (isAuthenticated) {
    return children;
  } else {
    return <Navigate to="/login" replace />;
  }

  return children;
};

export default PrivateRoute;
