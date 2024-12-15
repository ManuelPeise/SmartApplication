import React from "react";
import { Route, Router, Routes } from "react-router-dom";
import { BrowserHistory } from "history";
import { createBrowserHistory } from "history";
import Layout from "src/_components/_containers/Layout";
import Home from "src/_Stacks/PublicStack/Home";
import LogPageContainer from "src/_Stacks/_Administration/_Logging/LogPageContainer";

type IProps = {
  basename?: string;
  children: React.ReactNode;
  history: BrowserHistory;
};

export const routes = {
  home: "/",
  log: "/private/log",
};
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
          path="/private"
          element={<Layout isPrivate={true} history={history} />}
        >
          <Route path={routes.log} Component={LogPageContainer} />
        </Route>
      </Routes>
    </CustomBrowserRouter>
  );
};

export default AppRouter;
