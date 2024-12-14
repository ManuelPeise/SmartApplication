import {
  AccountCircleRounded,
  Menu,
  ExitToAppRounded,
} from "@material-ui/icons";
import {
  AppBar,
  Button,
  Grid2,
  IconButton,
  Toolbar,
  Typography,
} from "@mui/material";
import { BrowserHistory } from "history";
import React from "react";
import { useAuth } from "src/_hooks/useAuth";
import { useI18n } from "src/_hooks/useI18n";

const getAppbarTitle = (location: string) => {
  switch (location) {
    case "/":
      return "Home";
    default:
      return "";
  }
};

interface IProps {
  history: BrowserHistory;
  loginDialogOpen: boolean;
  registerDialogOpen: boolean;
  onLoginDialogOpen: () => void;
  onRegisterDialogOpen: () => void;
}

const AppHeaderBar: React.FC<IProps> = (props) => {
  const {
    history,
    loginDialogOpen,
    registerDialogOpen,
    onLoginDialogOpen,
    onRegisterDialogOpen,
  } = props;
  const { getResource } = useI18n();
  const { authenticationState, onLogout } = useAuth();

  return (
    <AppBar
      position="relative"
      style={{ backgroundColor: "#00004d", width: "100%" }}
    >
      <Toolbar>
        <IconButton size="large" color="inherit">
          <Menu />
        </IconButton>
        <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
          {getAppbarTitle(history.location.pathname)}
        </Typography>
        {authenticationState != null ? (
          <Grid2 display="flex" alignItems="center" flexDirection="row">
            <AccountCircleRounded style={{ width: "2rem", height: "2rem" }} />
            <Typography variant="h5" marginLeft="7px">
              {authenticationState.jwtData.name}
            </Typography>
            <IconButton
              size="large"
              color="inherit"
              onClick={onLogout.bind(null, authenticationState.jwtData.userId)}
            >
              <ExitToAppRounded style={{ width: "2rem", height: "2rem" }} />
            </IconButton>
          </Grid2>
        ) : (
          <Grid2 display="flex" alignItems="baseline" flexDirection="row">
            <Button
              disabled={registerDialogOpen}
              onClick={onRegisterDialogOpen}
            >
              <Typography>{getResource("common.labelRegister")}</Typography>
            </Button>
            <Typography variant="h5">|</Typography>
            <Button disabled={loginDialogOpen} onClick={onLoginDialogOpen}>
              <Typography>{getResource("common.labelLogin")}</Typography>
            </Button>
          </Grid2>
        )}
      </Toolbar>
    </AppBar>
  );
};

export default AppHeaderBar;
