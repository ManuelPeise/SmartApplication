import {
  AppsOutlined,
  EmailOutlined,
  SettingsOutlined,
} from "@material-ui/icons";
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
    spamMailClassification: "/configuration/spam-email-classification",
    tools: "/tools",
    emailCleaner: "/tools/email-cleaner",
    emailCleanerSettings: "/tools/email-cleaner-settings",
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
        requiredRole: UserRoleEnum.Admin || UserRoleEnum.User,
        childItems: null,
      },
      {
        displayNameRecourceKey: "common.labelSpamEmailClassification",
        route: routes.spamMailClassification,
        requiredRole: UserRoleEnum.Admin || UserRoleEnum.User,
        childItems: null,
      },
    ],
  },
  {
    displayNameRecourceKey: "common.labelEmailTools",
    requiredRole: UserRoleEnum.Admin || UserRoleEnum.User,
    route: "#",
    icon: <EmailOutlined style={{ width: "30px", height: "30px" }} />,
    childItems: [
      {
        displayNameRecourceKey: "common.labelEmailCleaner",
        route: routes.emailCleaner,
        requiredRole: UserRoleEnum.Admin || UserRoleEnum.User,
        childItems: null,
      },
      {
        displayNameRecourceKey: "common.labelEmailCleanerSettings",
        route: routes.emailCleaner,
        requiredRole: UserRoleEnum.Admin || UserRoleEnum.User,
        childItems: null,
      },
    ],
  },
];
