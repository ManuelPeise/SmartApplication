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
import EmailCleanerSettingsContainer from "src/Pages/Settings/EmailCleanerSettings/EmailCleanerSettingsContainer";
import EmailAddressMappingPageContainer from "src/Pages/Settings/EmailAddressMapping/EmailAddressMappingPageContainer";
import EmailCleanerPageContainer from "src/Pages/EmailCleaner/EmailCleanerPage";
import SandboxPage from "src/Pages/Sandbox/SandboxPage";

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

          <Route
            path={browserRoutes.home}
            element={
              <ProtectedRoute requiredRight={UserRightTypeEnum.MessageLog} />
            }
          >
            <Route path={browserRoutes.log} Component={LogPageContainer} />
          </Route>
          <Route
            path={browserRoutes.home}
            element={
              <ProtectedRoute
                requiredRight={UserRightTypeEnum.UserAdministration}
              />
            }
          >
            <Route
              path={browserRoutes.userAdministration}
              Component={UserAdministrationPageContainer}
            />
          </Route>
          <Route
            path={browserRoutes.home}
            element={
              <ProtectedRoute
                requiredRight={UserRightTypeEnum.EmailAccountSettings}
              />
            }
          >
            {/* <Route
              path={browserRoutes.emailAccountSettings}
              Component={EmailAccountSettingsPageContainer}
            /> */}
          </Route>
          <Route
            path={browserRoutes.home}
            element={
              <ProtectedRoute
                requiredRight={UserRightTypeEnum.EmailCleanerSettings}
              />
            }
          >
            <Route
              path={browserRoutes.emailCleanerSettings}
              Component={EmailCleanerSettingsContainer}
            />
            <Route
              path={`${browserRoutes.emailCleanerMappings}:id`}
              Component={EmailAddressMappingPageContainer}
            />
          </Route>
          <Route
            path={browserRoutes.home}
            element={
              <ProtectedRoute
                requiredRight={UserRightTypeEnum.EmailCleanerSettings}
              />
            }
          >
            <Route
              path={browserRoutes.emailCleaner}
              Component={EmailCleanerPageContainer}
            />
          </Route>
          <Route path="/sandbox" Component={SandboxPage} />
        </Routes>
      </PageLayout>
    </BrowserRouter>
  );
};

export default AppRouter;
