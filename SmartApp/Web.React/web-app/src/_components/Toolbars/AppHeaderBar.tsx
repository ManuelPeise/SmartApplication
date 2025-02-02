import {
  AccountCircleRounded,
  ExitToAppRounded,
  MenuRounded,
} from "@mui/icons-material";
import {
  AppBar,
  Box,
  Button,
  Grid2,
  IconButton,
  Toolbar,
  Typography,
} from "@mui/material";
import React from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { useAuth } from "src/_hooks/useAuth";
import { useI18n } from "src/_hooks/useI18n";
import { browserRoutes } from "src/_lib/Router/RouterUtils";

interface IProps {
  onOpenSideMenu: React.Dispatch<React.SetStateAction<boolean>>;
}

const AppHeaderBar: React.FC<IProps> = (props) => {
  const { onOpenSideMenu } = props;
  const { getResource } = useI18n();
  const { authenticationState, onLogout } = useAuth();
  const location = useLocation();
  const navigate = useNavigate();

  const withReplacedPath = React.useCallback((path: string, route: string) => {
    return path.replace(route, route);
  }, []);

  const pageTitle = React.useMemo((): string => {
    console.log(location.pathname);
    switch (location.pathname) {
      case browserRoutes.home:
        return `${process.env.REACT_APP_Name} - Home`;
      case browserRoutes.login:
        return `${process.env.REACT_APP_Name} - Login`;
      case browserRoutes.requestAccount:
        return `${process.env.REACT_APP_Name} - ${getResource(
          "common.labelRequestAccount"
        )}`;
      case browserRoutes.log:
        return `${process.env.REACT_APP_Name} - Log`;
      case browserRoutes.userAdministration:
        return `${process.env.REACT_APP_Name} - User Administration`;
      case browserRoutes.emailAccountInterface:
        return `${process.env.REACT_APP_Name} - Email Accounts`;
      case browserRoutes.emailCleanerInterface:
        return `${process.env.REACT_APP_Name} - Email Cleaner`;
      case withReplacedPath(
        location.pathname,
        browserRoutes.emailClassification
      ):
        return `${process.env.REACT_APP_Name} - Email Classification`;
      default:
        return "";
    }
  }, [location.pathname, getResource, withReplacedPath]);

  return (
    <AppBar
      id="app-tool-bar"
      position="relative"
      style={{ height: "6vh", backgroundColor: "#00004d", width: "100%" }}
    >
      <Toolbar sx={{ display: "flex", justifyContent: "space-between" }}>
        <Box>
          <IconButton
            size="medium"
            color="secondary"
            disabled={!authenticationState?.isAuthenticated}
            onClick={onOpenSideMenu.bind(null, true)}
          >
            <MenuRounded />
          </IconButton>

          <Button onClick={() => navigate(browserRoutes.home)}>
            <Typography
              variant="h6"
              component="div"
              marginLeft="10px"
              sx={{ flexGrow: 1, color: "#fff" }}
            >
              {pageTitle}
            </Typography>
          </Button>
        </Box>
        {authenticationState?.isAuthenticated === true ? (
          <Grid2 display="flex" alignItems="center" flexDirection="row">
            <AccountCircleRounded style={{ width: "2rem", height: "2rem" }} />
            <Typography variant="h5" marginLeft="7px">
              {authenticationState?.jwtData?.name}
            </Typography>
            <IconButton
              size="large"
              color="inherit"
              onClick={onLogout.bind(
                null,
                authenticationState?.jwtData?.userId
              )}
            >
              <ExitToAppRounded style={{ width: "2rem", height: "2rem" }} />
            </IconButton>
          </Grid2>
        ) : (
          <Grid2 display="flex" alignItems="baseline" flexDirection="row">
            {!location.pathname.endsWith("request-account") && (
              <Button
                style={{
                  color: "#fff",
                  textDecoration: "none",
                  padding: "5px 10px",
                }}
                onClick={() => navigate("/request-account")}
              >
                <Typography
                  sx={{ fontSize: "1.1rem", "&:hover": { opacity: 0.6 } }}
                >
                  {getResource("common.labelRequestAccount")}
                </Typography>
              </Button>
            )}
            {!location.pathname.endsWith("login") && (
              <Button
                style={{
                  color: "#fff",
                  textDecoration: "none",
                  padding: "5px 10px",
                }}
                onClick={() => navigate("/login")}
              >
                <Typography
                  sx={{ fontSize: "1.1rem", "&:hover": { opacity: 0.6 } }}
                >
                  {getResource("common.labelLogin")}
                </Typography>
              </Button>
            )}
          </Grid2>
        )}
      </Toolbar>
    </AppBar>
  );
};

export default AppHeaderBar;
