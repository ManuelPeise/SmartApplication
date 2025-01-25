import {
  HomeRepairServiceRounded,
  PolylineRounded,
  SettingsRounded,
} from "@mui/icons-material";
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

export const getToolsSideMenuItem = (
  accessRights: AccessRight[]
): SideMenuEntry | null => {
  if (accessRights == null || !accessRights.some((x) => x.canView)) {
    return null;
  }

  const item: SideMenuEntry = {
    displayNameRecourceKey: "common.labelTools",
    route: "#",
    icon: (
      <HomeRepairServiceRounded style={{ width: "30px", height: "30px" }} />
    ),
    childItems: [],
  };
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

  return item;
};

export const getInterfaceSideMenuItem = (
  accessRights: AccessRight[]
): SideMenuEntry | null => {
  if (accessRights == null || !accessRights.some((x) => x.canView)) {
    return null;
  }

  const item: SideMenuEntry = {
    displayNameRecourceKey: "common.labelInterface",
    route: "#",
    icon: <PolylineRounded style={{ width: "30px", height: "30px" }} />,
    childItems: [],
  };

  const emailAccountInterfaceRight =
    accessRights.find((x) => x.name === "EmailAccountInterface") ?? null;

  if (
    emailAccountInterfaceRight != null &&
    emailAccountInterfaceRight.canView
  ) {
    item.childItems.push({
      displayNameRecourceKey: "common.labelEmailAccountInterface",
      route: browserRoutes.emailAccountInterface,
      childItems: null,
    });
  }
  return item;
};
