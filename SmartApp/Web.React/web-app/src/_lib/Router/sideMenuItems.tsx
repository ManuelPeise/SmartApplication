import React from "react";
import { HomeRepairServiceRounded, SettingsRounded } from "@mui/icons-material";
import { browserRoutes } from "./RouterUtils";
import { AccessRight } from "../_types/auth";

export type SideMenuEntry = {
  displayNameRecourceKey: string;
  route: string;
  icon?: JSX.Element;
  childItems?: SideMenuEntry[] | null;
};

export const getAdministrationSideMenuItem = (
  accessRights: AccessRight[]
): SideMenuEntry | null => {
  if (accessRights == null || !accessRights.some((x) => x.canView)) {
    return null;
  }

  const item: SideMenuEntry = {
    displayNameRecourceKey: "common.labelAdministration",
    route: "#",
    icon: (
      <HomeRepairServiceRounded style={{ width: "30px", height: "30px" }} />
    ),
    childItems: [],
  };

  const logRight = accessRights.find((x) => x.name === "MessageLog") ?? null;

  if (logRight != null && logRight.canView) {
    item.childItems.push({
      displayNameRecourceKey: "common.labelLogging",
      route: browserRoutes.log,
      childItems: null,
    });
  }

  const userAdministrationRight =
    accessRights.find((x) => x.name === "UserAdministration") ?? null;

  if (userAdministrationRight != null && userAdministrationRight.canView) {
    item.childItems.push({
      displayNameRecourceKey: "common.labelUserAdministration",
      route: browserRoutes.userAdministration,
      childItems: null,
    });
  }
  return item;
};

export const getSettingsSideMenuItem = (
  accessRights: AccessRight[]
): SideMenuEntry | null => {
  if (accessRights == null || !accessRights.some((x) => x.canView)) {
    return null;
  }

  const item: SideMenuEntry = {
    displayNameRecourceKey: "common.labelSettings",
    route: "#",
    icon: <SettingsRounded style={{ width: "30px", height: "30px" }} />,
    childItems: [],
  };

  const emailAccountSettingsRight =
    accessRights.find((x) => x.name === "EmailAccountSettings") ?? null;

  if (emailAccountSettingsRight != null && emailAccountSettingsRight.canView) {
    item.childItems.push({
      displayNameRecourceKey: "common.labelEmailAccountSettings",
      route: browserRoutes.emailAccountSettings,
      childItems: null,
    });
  }

  return item;
};
