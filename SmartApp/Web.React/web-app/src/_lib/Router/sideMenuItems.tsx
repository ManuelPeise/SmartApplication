import React from "react";

import { UserRoleEnum } from "../_enums/UserRoleEnum";
import { HomeRepairServiceRounded, SettingsRounded } from "@mui/icons-material";
import { browserRoutes } from "./RouterUtils";

export type SideMenuEntry = {
  displayNameRecourceKey: string;
  route: string;
  requiredRole: UserRoleEnum;
  icon?: JSX.Element;
  childItems?: SideMenuEntry[] | null;
};

export const getSideMenuItems = (): SideMenuEntry[] => {
  return [
    {
      displayNameRecourceKey: "common.labelAdministration",
      requiredRole: UserRoleEnum.Admin,
      route: "#",
      icon: (
        <HomeRepairServiceRounded style={{ width: "30px", height: "30px" }} />
      ),
      childItems: [
        {
          displayNameRecourceKey: "common.labelLogging",
          route: browserRoutes.log,
          requiredRole: UserRoleEnum.Admin,
          childItems: null,
        },
        {
          displayNameRecourceKey: "common.labelUserAdministration",
          route: browserRoutes.userAdministration,
          requiredRole: UserRoleEnum.Admin,
          childItems: null,
        },
      ],
    },
    {
      displayNameRecourceKey: "common.labelSettings",
      requiredRole: UserRoleEnum.Admin || UserRoleEnum.User,
      route: "#",
      icon: <SettingsRounded style={{ width: "30px", height: "30px" }} />,
      childItems: [
        {
          displayNameRecourceKey: "common.labelEmailAccountSettings",
          route: browserRoutes.emailAccountSettings,
          requiredRole: UserRoleEnum.Admin,
          childItems: null,
        },
      ],
    },
  ];
};
