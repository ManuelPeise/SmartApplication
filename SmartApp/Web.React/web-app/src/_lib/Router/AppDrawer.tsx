import { ArrowBackRounded } from "@mui/icons-material";
import { Box, Drawer, IconButton, List } from "@mui/material";
import React from "react";
import DrawerListItem from "./DrawerListItem";
import {
  getAdministrationSideMenuItem,
  getInterfaceSideMenuItem,
  SideMenuEntry,
} from "./sideMenuItems";
import { useI18n } from "src/_hooks/useI18n";
import { useAccessRights } from "src/_hooks/useAccessRights";

interface IProps {
  open: boolean;
  onClose: () => void;
}

const AppDrawer: React.FC<IProps> = (props) => {
  const { open, onClose } = props;
  const { getResource } = useI18n();
  const { accessRights } = useAccessRights();

  const sideMenueListItems = React.useMemo((): SideMenuEntry[] => {
    const items: SideMenuEntry[] = [];

    const administrationSideMenuItem = getAdministrationSideMenuItem(
      accessRights?.accessRights.filter((x) => x.group === "Administration")
    );

    if (administrationSideMenuItem != null) {
      items.push(administrationSideMenuItem);
    }

    const interfaceSideMenuItem = getInterfaceSideMenuItem(
      accessRights?.accessRights.filter((x) => x.group === "Interface")
    );

    if (interfaceSideMenuItem != null) {
      items.push(interfaceSideMenuItem);
    }

    // const settingsSideMenuItem = getSettingsSideMenuItem(
    //   accessRights?.accessRights.filter((x) => x.group === "Settings")
    // );
    // if (settingsSideMenuItem != null) {
    //   items.push(settingsSideMenuItem);
    // }

    // const toolsSideMenuItem = getToolsSideMenuItem(
    //   accessRights?.accessRights.filter((x) => x.group === "Tools")
    // );

    // if (toolsSideMenuItem != null) {
    //   items.push(toolsSideMenuItem);
    // }

    return items;
  }, [accessRights]);

  return (
    <Drawer
      anchor="left"
      open={open}
      sx={{ width: "100%", zIndex: 1000 }}
      slotProps={{
        backdrop: {
          sx: {
            backgroundColor: "transparent",
            opacity: 0.5,
          },
        },
      }}
    >
      <Box
        display="flex"
        justifyContent="flex-end"
        padding={2}
        bgcolor="primary.dark"
      >
        <IconButton size="small" color="secondary" onClick={onClose}>
          <ArrowBackRounded />
        </IconButton>
      </Box>
      <Box height="100%" bgcolor="primary.dark">
        <List
          style={{
            width: "100%",
            backgroundColor: "primary.dark",
          }}
        >
          {sideMenueListItems.map((item, key) => {
            return (
              <DrawerListItem
                key={key}
                {...item}
                onCloseMenu={onClose}
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
