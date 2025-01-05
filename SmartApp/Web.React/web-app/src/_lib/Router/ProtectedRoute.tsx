import React from "react";
import { RouteProps } from "./routeTypes";
import { useAuth } from "src/_hooks/useAuth";
import { Navigate, Outlet } from "react-router-dom";
import { UserRoleEnum } from "../_enums/UserRoleEnum";

const ProtectedRoute: React.FC<RouteProps> = (props) => {
  const { redirectUri, requiredRole } = props;
  const auth = useAuth();

  const roleKey =
    (auth.authenticationState.jwtData.userRole as UserRoleEnum) ===
    UserRoleEnum.Admin
      ? 1
      : 0;

  if (
    !auth.authenticationState.isAuthenticated ||
    (requiredRole === UserRoleEnum.Admin && roleKey === 0)
  ) {
    return <Navigate to={redirectUri} />;
  }

  return <Outlet />;
};

export default ProtectedRoute;
