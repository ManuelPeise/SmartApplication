import React from "react";
import { AuthContext } from "src/_providers/AuthContextProvider";

export const useAuth = () => {
  return React.useContext(AuthContext);
};
