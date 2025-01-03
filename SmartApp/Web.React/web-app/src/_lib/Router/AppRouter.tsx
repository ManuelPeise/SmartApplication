import React from "react";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import PageLayout from "./PageLayout";
import LoginPage from "src/Pages/Auth/LoginPage";
import AdminRoute from "./AdminRoute";
import LogPageContainer from "src/_Stacks/_Administration/_Logging/LogPageContainer";
import { UserRoleEnum } from "../_enums/UserRoleEnum";
import { browserRoutes } from "./RouterUtils";
import Home from "src/_Stacks/PublicStack/Home";

const AppRouter: React.FC = () => {
  return (
    <BrowserRouter>
      <PageLayout>
        <Routes>
          <Route path={browserRoutes.login} Component={LoginPage} />
          <Route path={browserRoutes.register} element={<div></div>} />
          <Route
            path="/"
            element={
              <AdminRoute
                redirectUri={browserRoutes.login}
                requiredRole={UserRoleEnum.User}
              />
            }
          >
            <Route path={browserRoutes.home} Component={Home} />
          </Route>
          <Route
            path={browserRoutes.home}
            element={
              <AdminRoute
                redirectUri={browserRoutes.login}
                requiredRole={UserRoleEnum.Admin}
              />
            }
          >
            <Route path={browserRoutes.log} Component={LogPageContainer} />
          </Route>
        </Routes>
      </PageLayout>
    </BrowserRouter>
  );
};

export default AppRouter;
