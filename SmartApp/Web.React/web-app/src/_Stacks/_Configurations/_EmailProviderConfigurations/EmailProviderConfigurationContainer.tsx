import React from "react";
import EmailConfigurationContextProvider from "src/_providers/EmailConfigurationContextProvider";
import EmailProviderConfigurationPage from "./EmailProviderConfigurationPage";

const EmailProviderConfigurationContainer: React.FC = () => {
  return (
    <EmailConfigurationContextProvider>
      <EmailProviderConfigurationPage />
    </EmailConfigurationContextProvider>
  );
};

export default EmailProviderConfigurationContainer;
