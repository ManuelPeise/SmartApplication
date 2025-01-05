import React from "react";
import { AuthContext } from "src/_providers/AuthContextProvider";

export const useAuth = () => {
  const context = React.useContext(AuthContext);

  if (!context == null) {
    throw new Error("Auth context is null!");
  }

  return context;
};
