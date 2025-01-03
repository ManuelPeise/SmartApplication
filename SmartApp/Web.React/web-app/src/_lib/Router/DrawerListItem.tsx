import React from "react";
import { UserRoleEnum } from "../_enums/UserRoleEnum";
import {
  Box,
  List,
  ListItem,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Typography,
} from "@mui/material";
import { colors } from "../colors";
import { Link } from "react-router-dom";
import { SideMenuEntry } from "../sideMenuItems";

interface ISideMenuItemProps extends SideMenuEntry {
  userRole: UserRoleEnum;
  getResource: (key: string) => string;
  onCloseMenu: () => void;
}

const DrawerListItem: React.FC<ISideMenuItemProps> = (props) => {
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

export default DrawerListItem;
