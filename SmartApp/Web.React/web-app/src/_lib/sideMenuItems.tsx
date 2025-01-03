import React from "react";

import { UserRoleEnum } from "./_enums/UserRoleEnum";
import { Routes } from "src/types";
import { SettingsRounded } from "@mui/icons-material";

export type SideMenuEntry = {
  displayNameRecourceKey: string;
  route: string;
  requiredRole: UserRoleEnum;
  icon?: JSX.Element;
  childItems?: SideMenuEntry[] | null;
};

export const getRoutes = (): Routes => {
  return {
    administration: "/administration",
    log: "/administration/log",
    home: "/",
    private: "/private",
  };
};

export const getSideMenuItems = (routes: Routes): SideMenuEntry[] => [
  {
    displayNameRecourceKey: "common.labelAdministration",
    requiredRole: UserRoleEnum.Admin,
    route: "#",
    icon: <SettingsRounded style={{ width: "30px", height: "30px" }} />,
    childItems: [
      {
        displayNameRecourceKey: "common.labelLogging",
        route: routes.log,
        requiredRole: UserRoleEnum.Admin,
        childItems: null,
      },
    ],
  },
];
