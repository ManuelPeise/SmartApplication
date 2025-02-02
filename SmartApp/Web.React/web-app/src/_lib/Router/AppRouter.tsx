import React from "react";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import PageLayout from "./PageLayout";
import LoginPage from "src/Pages/Auth/LoginPage";
import ProtectedRoute from "./ProtectedRoute";
import LogPageContainer from "src/Pages/Administration/Logging/LogPageContainer";
import { browserRoutes } from "./RouterUtils";
import Home from "src/_Stacks/PublicStack/Home";
import RequestAccountPage from "src/Pages/Auth/RequestAccountPage";
import UserAdministrationPageContainer from "src/Pages/Administration/UserAdministration/UserAdministrationPageContainer";
import { UserRightTypeEnum } from "./routeTypes";
import SandboxPage from "src/Pages/Sandbox/SandboxPage";
import EmailAccountInterfaceContainer from "src/Pages/Interfaces/EmailAccountInterface/EmailAccountInterfaceContainer";
import EmailCleanerInterfaceContainer from "src/Pages/Interfaces/EmailCleanerInterface/EmailCleanerInterfaceContainer";
import EmailClassificationContainer from "src/Pages/Interfaces/EmailClassification/EmailClassificationContainer";

const AppRouter: React.FC = () => {
  return (
    <BrowserRouter>
      <PageLayout>
        <Routes>
          <Route path={browserRoutes.login} Component={LoginPage} />

          <Route
            path={browserRoutes.requestAccount}
            Component={RequestAccountPage}
          />

          <Route path="/" element={<ProtectedRoute />}>
            <Route path={browserRoutes.home} Component={Home} />
          </Route>
          {/* Administration */}
          <Route
            path={browserRoutes.home}
            element={
              <ProtectedRoute
                requiredRight={UserRightTypeEnum.UserAdministration}
              />
            }
          >
            <Route path={browserRoutes.log} Component={LogPageContainer} />
            <Route
              path={browserRoutes.userAdministration}
              Component={UserAdministrationPageContainer}
            />
          </Route>
          {/* Interfaces */}
          <Route
            path={browserRoutes.home}
            element={
              <ProtectedRoute
                requiredRight={UserRightTypeEnum.EmailAccountInterface}
              />
            }
          >
            <Route
              path={browserRoutes.emailAccountInterface}
              Component={EmailAccountInterfaceContainer}
            />
            <Route
              path={browserRoutes.emailCleanerInterface}
              Component={EmailCleanerInterfaceContainer}
            />
            <Route
              path={browserRoutes.emailClassification}
              Component={EmailClassificationContainer}
            />
          </Route>
          <Route path="/sandbox" Component={SandboxPage} />
        </Routes>
      </PageLayout>
    </BrowserRouter>
  );
};

export default AppRouter;
