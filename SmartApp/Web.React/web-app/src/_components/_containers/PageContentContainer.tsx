import {
  Box,
  Drawer,
  IconButton,
  List,
  ListItem,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Typography,
} from "@mui/material";
import React, { PropsWithChildren } from "react";
import { Link } from "react-router-dom";
import { useAuth } from "src/_hooks/useAuth";
import { UserRoleEnum } from "src/_lib/_enums/UserRoleEnum";
import { colors } from "src/_lib/colors";
import { useI18n } from "src/_hooks/useI18n";
import {
  SideMenuEntry,
  getRoutes,
  getSideMenuItems,
} from "src/_lib/sideMenuItems";
import { ArrowBackRounded } from "@mui/icons-material";

interface IProps extends PropsWithChildren {
  sideMenuOpen: boolean;
  onClose: React.Dispatch<React.SetStateAction<boolean>>;
}

interface ISideMenuItemProps extends SideMenuEntry {
  userRole: UserRoleEnum;
  getResource: (key: string) => string;
  onCloseMenu: () => void;
}

const SideMenuItem: React.FC<ISideMenuItemProps> = (props) => {
  const {
    displayNameRecourceKey,
    route,
    requiredRole,
    userRole,
    icon,
    childItems,
    getResource,
    onCloseMenu,
  } = props;

  const [expanded, setExpanded] = React.useState<boolean>(false);

  const onClickHandler = childItems != null ? setExpanded : null;

  return (
    <ListItemButton
      disabled={userRole === undefined || userRole !== requiredRole}
      sx={{
        display: "flex",
        flexDirection: "column",
        color: "#fff",
      }}
      onClick={onClickHandler.bind(null, !expanded)}
    >
      <Box display="flex" flexDirection="row" alignItems="center" width="100%">
        <ListItemIcon
          sx={{
            color: colors.typography.white,
            display: "flex",
            justifyContent: "center",
            alignItems: "center",
          }}
        >
          {icon}
        </ListItemIcon>
        {childItems == null ? (
          <Link style={{ height: "30xp" }} to={route} />
        ) : (
          <ListItemText style={{ display: "flex", alignItems: "center" }}>
            <Typography sx={{ fontSize: "1rem" }}>
              {getResource(displayNameRecourceKey)}
            </Typography>
          </ListItemText>
        )}
      </Box>
      <Box display="flex" flexDirection="column" width="100%">
        <List sx={{ paddingLeft: "1.5rem", width: "100%" }}>
          {expanded &&
            childItems.map((item, key) => (
              <ListItem
                sx={{
                  paddingLeft: "2rem",
                  paddingTop: 1,
                  paddingBottom: 1,
                  "&:hover": {
                    opacity: 0.7,
                  },
                }}
                onClick={onCloseMenu}
              >
                <ListItemText>
                  <Link
                    to={item.route}
                    style={{
                      textDecoration: "none",
                      color: colors.typography.white,
                    }}
                  >
                    <Typography sx={{ fontSize: "1rem" }}>
                      {getResource(item.displayNameRecourceKey)}
                    </Typography>
                  </Link>
                </ListItemText>
              </ListItem>
            ))}
        </List>
      </Box>
    </ListItemButton>
  );
};

const PageContentContainer: React.FC<IProps> = (props) => {
  const { children, sideMenuOpen, onClose } = props;
  const { authenticationState } = useAuth();
  const { getResource } = useI18n();

  const sideMenuItems = React.useMemo(() => {
    return getSideMenuItems(getRoutes());
  }, []);
  return (
    <Box
      display="flex"
      flexDirection="row"
      height="100%"
      bgcolor={colors.background}
    >
      <Drawer
        anchor="left"
        open={sideMenuOpen}
        slotProps={{
          backdrop: {
            sx: {
              backgroundColor: "transparent",
              opacity: 0.5,
            },
          },
        }}
        sx={{ zIndex: 1000 }}
      >
        <Box
          display="flex"
          justifyContent="flex-end"
          padding={2}
          bgcolor="primary.dark"
        >
          <IconButton
            size="small"
            color="secondary"
            onClick={onClose.bind(null, false)}
          >
            <ArrowBackRounded />
          </IconButton>
        </Box>
        <Box height="100%" width="300px" bgcolor="primary.dark">
          <List
            disablePadding
            style={{
              width: "100%",
            }}
          >
            {sideMenuItems.map((item, key) => {
              return (
                <SideMenuItem
                  key={key}
                  {...item}
                  userRole={
                    authenticationState?.jwtData?.userRole as UserRoleEnum
                  }
                  onCloseMenu={onClose.bind(null, false)}
                  getResource={getResource}
                />
              );
            })}
          </List>
        </Box>
      </Drawer>
      {/* content */}
      <Box display="flex" width="100%">
        {children}
      </Box>
    </Box>
  );
};

export default PageContentContainer;
