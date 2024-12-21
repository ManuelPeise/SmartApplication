import React from "react";
import { Route, Router, Routes } from "react-router-dom";
import { BrowserHistory } from "history";
import { createBrowserHistory } from "history";
import Layout from "src/_components/_containers/Layout";
import Home from "src/_Stacks/PublicStack/Home";
import LogPageContainer from "src/_Stacks/_Administration/_Logging/LogPageContainer";
import EmailProviderConfigurationContainer from "src/_Stacks/_Configurations/_EmailProviderConfigurations/EmailProviderConfigurationContainer";
import { getRoutes } from "./sideMenuItems";

type IProps = {
  basename?: string;
  children: React.ReactNode;
  history: BrowserHistory;
};

export const routes = getRoutes();

const CustomBrowserRouter = ({ basename, children, history }: IProps) => {
  const [state, setState] = React.useState({
    action: history.action,
    location: history.location,
  });

  React.useLayoutEffect(() => history.listen(setState), [history]);

  return (
    <Router
      basename={basename}
      location={state.location}
      navigator={history}
      navigationType={state.action}
    >
      {children}
    </Router>
  );
};

const AppRouter: React.FC = () => {
  const history = createBrowserHistory();

  return (
    <CustomBrowserRouter history={history}>
      <Routes>
        <Route
          path={routes.home}
          element={<Layout isPrivate={false} history={history} />}
        >
          <Route path={routes.home} Component={Home} />
        </Route>
        <Route
          path="/administration"
          element={<Layout isPrivate={true} history={history} />}
        >
          <Route path={routes.log} Component={LogPageContainer} />
        </Route>
        <Route
          path={routes.configuration}
          element={<Layout isPrivate={true} history={history} />}
        >
          <Route
            path={routes.emailProviderConfiguration}
            Component={EmailProviderConfigurationContainer}
          />
        </Route>
      </Routes>
    </CustomBrowserRouter>
  );
};

export default AppRouter;
