import React from "react";
import { AccessRightContext } from "src/Providers/AccessRightProvider";

export const useAccessRights = () => {
  const context = React.useContext(AccessRightContext);

  if (!context) {
    throw new Error("Access right context is null!");
  }

  return context;
};
