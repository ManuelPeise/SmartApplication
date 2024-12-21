import { Box } from "@mui/material";
import React from "react";
import { Outlet } from "react-router-dom";
import { useAuth } from "src/_hooks/useAuth";
import AuthenticationDialog from "../dialogs/AuthenticationDialog";
import { BrowserHistory } from "history";
import AppHeaderBar from "../_toolbars/AppHeaderBar";
import RegisterDialog from "../dialogs/RegisterDialog";
import PageContentContainer from "./PageContentContainer";

interface IProps {
  isPrivate: boolean;
  history: BrowserHistory;
}

const Layout: React.FC<IProps> = (props) => {
  const { isPrivate, history } = props;
  const { authenticationState } = useAuth();

  const [sideMenuOpen, setSideMenuOpen] = React.useState<boolean>(false);
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
    <Box
      width="100%"
      height="100%"
      padding={0}
      margin={0}
      display="flex"
      flexDirection="column"
    >
      <AppHeaderBar
        history={history}
        loginDialogOpen={authDialogOpen}
        registerDialogOpen={registerDialogOpen}
        onOpenSideMenu={setSideMenuOpen}
        onLoginDialogOpen={setAuthDialogOpen.bind(null, true)}
        onRegisterDialogOpen={setRegisterDialogOpen.bind(null, true)}
      />
      <PageContentContainer
        sideMenuOpen={sideMenuOpen}
        onClose={setSideMenuOpen}
      >
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
      </PageContentContainer>
    </Box>
  );
};

export default Layout;
