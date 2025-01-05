import { ArrowBackRounded } from "@mui/icons-material";
import { Box, Drawer, IconButton, List } from "@mui/material";
import React from "react";
import DrawerListItem from "./DrawerListItem";
import { getSideMenuItems } from "./sideMenuItems";
import { useI18n } from "src/_hooks/useI18n";
import { UserRoleEnum } from "../_enums/UserRoleEnum";
import { useAuth } from "src/_hooks/useAuth";

interface IProps {
  open: boolean;
  onClose: () => void;
}

const AppDrawer: React.FC<IProps> = (props) => {
  const { open, onClose } = props;
  const { getResource } = useI18n();
  const { authenticationState } = useAuth();

  const sideMenuItems = React.useMemo(() => {
    return getSideMenuItems();
  }, []);

  return (
    <Drawer
      anchor="left"
      open={open}
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
              <DrawerListItem
                key={key}
                {...item}
                userRole={authenticationState.jwtData.userRole as UserRoleEnum}
                onCloseMenu={onClose.bind(null, false)}
                getResource={getResource}
              />
            );
          })}
        </List>
      </Box>
    </Drawer>
  );
};

export default AppDrawer;
