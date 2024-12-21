import React from "react";
import {
  EmailConfigurationContext,
  EmailConfigurationConxtextProps,
} from "src/_providers/EmailConfigurationContextProvider";

export const useEmailConfigurationContextProvider = () => {
  const context = React.useContext<EmailConfigurationConxtextProps>(
    EmailConfigurationContext
  );

  if (!context) {
    throw new Error(
      "useEmailConfigurationContextProvider must be used within a Provider"
    );
  }

  return context;
};
