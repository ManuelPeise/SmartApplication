import {
  Grid2,
  IconButton,
  List,
  ListItemButton,
  Typography,
} from "@mui/material";
import {
  ArrowBackRounded,
  MenuOutlined,
  ReportProblemRounded,
  SettingsRounded,
} from "@material-ui/icons";
import React, { PropsWithChildren } from "react";
import { SideMenu } from "src/_lib/_types/menu";
import { colors } from "src/_lib/colors";
import { Link } from "react-router-dom";
import { useAuth } from "src/_hooks/useAuth";
import { UserRoleEnum } from "src/_lib/_enums/UserRoleEnum";
import { routes } from "src/_lib/AppRouter";

const sideMenuMinWidth = "50px";
const sideMenuExpandedWidth = "200px";

interface ISideMenuItemProps {
  displayName: string;
  route: string;
  disabled: boolean;
  isExpanded: boolean;
  icon: JSX.Element;
}

const SideMenuItem: React.FC<ISideMenuItemProps> = (props) => {
  const { displayName, route, disabled, isExpanded, icon } = props;

  return (
    <ListItemButton disabled={disabled} style={{ width: "100%" }}>
      <Link to={route} style={{ textDecoration: "none", width: "100%" }}>
        <Grid2 display="flex" alignItems="baseline" width="100%">
          <Grid2
            display="flex"
            justifyItems="center"
            alignItems="baseline"
            width={sideMenuMinWidth}
          >
            {icon}
          </Grid2>
          <Grid2 display={isExpanded ? "block" : "none"}>
            <Typography style={{ fontSize: "18px", color: "#fff" }}>
              {displayName}
            </Typography>
          </Grid2>
        </Grid2>
      </Link>
    </ListItemButton>
  );
};

const PageContentContainer: React.FC<PropsWithChildren> = (props) => {
  const { children } = props;
  const { authenticationState } = useAuth();
  const [sideMenuExpanded, setSideMenuExpanded] =
    React.useState<boolean>(false);

  const appSideMenu = React.useMemo((): SideMenu => {
    return {
      items: [
        {
          displayName: "Logging",
          route: routes.log,
          disabled:
            authenticationState == null ||
            authenticationState.jwtData.userRole !== UserRoleEnum.Admin,
          icon: (
            <ReportProblemRounded fontSize="small" style={{ color: "#fff" }} />
          ),
        },
        {
          displayName: "Settings",
          route: routes.settings,
          disabled: authenticationState == null,
          icon: <SettingsRounded fontSize="small" style={{ color: "#fff" }} />,
        },
      ],
    };
  }, [authenticationState]);

  return (
    <Grid2 display="flex" flexDirection="row" height="100%">
      {/* sideMenu */}
      <Grid2
        bgcolor={colors.darkBlue}
        width={sideMenuExpanded ? sideMenuExpandedWidth : sideMenuMinWidth}
      >
        <Grid2
          display="flex"
          padding="5px 8px"
          justifyContent={sideMenuExpanded ? "flex-end" : "center"}
        >
          <IconButton
            size="large"
            style={{ color: "#fff" }}
            onClick={setSideMenuExpanded.bind(
              null,
              sideMenuExpanded ? false : true
            )}
          >
            {sideMenuExpanded ? (
              <ArrowBackRounded fontSize="small" />
            ) : (
              <MenuOutlined fontSize="small" />
            )}
          </IconButton>
        </Grid2>
        <Grid2>
          <List>
            {appSideMenu.items.map((item, key) => {
              return (
                <SideMenuItem
                  key={key}
                  {...item}
                  isExpanded={sideMenuExpanded}
                />
              );
            })}
          </List>
        </Grid2>
      </Grid2>
      {/* content */}
      <Grid2 display="flex" width="100%">
        {children}
      </Grid2>
    </Grid2>
  );
};

export default PageContentContainer;
