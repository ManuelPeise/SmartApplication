import { AppsOutlined, SettingsOutlined } from "@material-ui/icons";
import React from "react";

import { UserRoleEnum } from "./_enums/UserRoleEnum";
import { Routes } from "src/types";

export type SideMenuEntry = {
  displayNameRecourceKey: string;
  route: string;
  requiredRole: UserRoleEnum;
  icon?: JSX.Element;
  childItems?: SideMenuEntry[] | null;
};

export const getRoutes = (): Routes => {
  return {
    log: "/administration/log",
    home: "/",
    private: "/private",
    configuration: "/configuration",
    emailProviderConfiguration: "/configuration/email-provider",
  };
};

export const getSideMenuItems = (routes: Routes): SideMenuEntry[] => [
  {
    displayNameRecourceKey: "common.labelAdministration",
    requiredRole: UserRoleEnum.Admin,
    route: "#",
    icon: <SettingsOutlined style={{ width: "30px", height: "30px" }} />,
    childItems: [
      {
        displayNameRecourceKey: "common.labelLogging",
        route: routes.log,
        requiredRole: UserRoleEnum.Admin,
        childItems: null,
      },
    ],
  },
  {
    displayNameRecourceKey: "common.labelConfiguration",
    requiredRole: UserRoleEnum.Admin || UserRoleEnum.User,
    route: "#",
    icon: <AppsOutlined style={{ width: "30px", height: "30px" }} />,
    childItems: [
      {
        displayNameRecourceKey: "common.labelEmailProviderConfituration",
        route: routes.emailProviderConfiguration,
        requiredRole: UserRoleEnum.Admin,
        childItems: null,
      },
    ],
  },
];
