import React from "react";
import { RouteProps } from "./routeTypes";
import { useAuth } from "src/_hooks/useAuth";
import { Navigate, Outlet } from "react-router-dom";
import { browserRoutes } from "./RouterUtils";
import { useAccessRights } from "src/_hooks/useAccessRights";
import { AccessRight } from "../_types/auth";

const ProtectedRoute: React.FC<RouteProps> = (props) => {
  const { requiredRight } = props;
  const auth = useAuth();
  const { accessRights } = useAccessRights();

  console.log("Rights", accessRights);
  let hasAccess = false;

  const accessRight = React.useMemo((): AccessRight => {
    if (requiredRight === undefined) {
      return {
        id: -1,
        name: "",
        group: "",
        canEdit: true,
        canView: true,
        deny: false,
      };
    }
    return accessRights.accessRights.find((x) => x.name === requiredRight);
  }, [requiredRight, accessRights]);

  if (auth.authenticationState.isAuthenticated) {
    hasAccess = true;
  }

  if (accessRight.canView) {
    hasAccess = true;
  } else {
    hasAccess = false;
  }

  if (!hasAccess) {
    <Navigate to={browserRoutes.home} />;
  }

  return <Outlet />;
};

export default ProtectedRoute;
