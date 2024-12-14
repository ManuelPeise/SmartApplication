import { Grid2 } from "@mui/material";
import React from "react";
import { Outlet } from "react-router-dom";
import { useAuth } from "src/_hooks/useAuth";
import AuthenticationDialog from "../dialogs/AuthenticationDialog";
import { BrowserHistory } from "history";
import AppHeaderBar from "../_toolbars/AppHeaderBar";
import RegisterDialog from "../dialogs/RegisterDialog";

interface IProps {
  isPrivate: boolean;
  history: BrowserHistory;
}

const Layout: React.FC<IProps> = (props) => {
  const { isPrivate, history } = props;
  const { authenticationState } = useAuth();

  const [authDialogOpen, setAuthDialogOpen] = React.useState<boolean>(false);
  const [registerDialogOpen, setRegisterDialogOpen] =
    React.useState<boolean>(false);

  React.useEffect(() => {
    if (
      isPrivate === true &&
      (authenticationState == null || !authenticationState.token?.length)
    ) {
      setAuthDialogOpen(true);
    }
  }, [isPrivate, authenticationState]);

  const onCancelLogin = React.useCallback(() => {
    if (history.location.pathname.startsWith("/private")) {
      history.go(-1);
    }

    setAuthDialogOpen(false);
  }, [history]);

  return (
    <Grid2
      width="100%"
      style={{
        padding: 0,
        margin: 0,
        display: "flex",
        flexDirection: "column",
      }}
    >
      <Grid2>
        <AppHeaderBar
          history={history}
          loginDialogOpen={authDialogOpen}
          registerDialogOpen={registerDialogOpen}
          onLoginDialogOpen={setAuthDialogOpen.bind(null, true)}
          onRegisterDialogOpen={setRegisterDialogOpen.bind(null, true)}
        />
      </Grid2>
      <Grid2 height="100%">
        <Outlet />
        <AuthenticationDialog
          open={authDialogOpen}
          onCancel={onCancelLogin}
          onClose={setAuthDialogOpen.bind(null, false)}
        />
        <RegisterDialog
          open={registerDialogOpen}
          onClose={setRegisterDialogOpen.bind(null, false)}
        />
      </Grid2>
    </Grid2>
  );
};

export default Layout;
