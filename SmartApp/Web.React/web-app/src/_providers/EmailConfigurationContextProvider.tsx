import React, { PropsWithChildren } from "react";
import { useApi } from "src/_hooks/useApi";
import { useI18n } from "src/_hooks/useI18n";
import { SuccessResponse } from "src/_lib/_types/response";
import { EmailProviderConfiguration } from "src/_Stacks/_Configurations/_EmailProviderConfigurations/types";

export type EmailConfigurationConxtextProps = {
  isLoading: boolean;
  emailProviderConfigurations: EmailProviderConfiguration[];
  getResource: (key: string) => string;
  handleProviderConnectionTest: (
    configuration: EmailProviderConfiguration
  ) => Promise<boolean>;
  handleEstablishConnection: (
    configuration: EmailProviderConfiguration
  ) => Promise<void>;
};

export const EmailConfigurationContext =
  React.createContext<EmailConfigurationConxtextProps>(
    {} as EmailConfigurationConxtextProps
  );

const EmailConfigurationContextProvider: React.FC<PropsWithChildren> = (
  props
) => {
  const { children } = props;
  const { getResource } = useI18n();
  const { data, isLoading, sendGetRequest, sendPost } =
    useApi<EmailProviderConfiguration>({
      requestUrl: "EmailProvider/GetProviderSettings",
      isPrivate: true,
      parameters: null,
      initialLoad: true,
    });

  const handleProviderConnectionTest = React.useCallback(
    async (configuration: EmailProviderConfiguration): Promise<boolean> => {
      let success = false;

      await sendPost<SuccessResponse>({
        requestUrl: "EmailProvider/ProviderConnectionTest",
        data: JSON.stringify(configuration),
      }).then((res) => {
        success = res.success;
      });

      return success;
    },
    [sendPost]
  );

  const handleEstablishConnection = React.useCallback(
    async (configuration: EmailProviderConfiguration) => {
      await sendPost<SuccessResponse>({
        requestUrl: "EmailProvider/EstablishProviderConnection",
        data: JSON.stringify(configuration),
      }).then(async (res) => {
        if (res) {
          await sendGetRequest();
        }
      });
    },
    [sendGetRequest, sendPost]
  );

  return (
    <EmailConfigurationContext.Provider
      value={{
        isLoading: isLoading,
        emailProviderConfigurations: data,
        getResource: getResource,
        handleProviderConnectionTest: handleProviderConnectionTest,
        handleEstablishConnection: handleEstablishConnection,
      }}
    >
      {children}
    </EmailConfigurationContext.Provider>
  );
};

export default EmailConfigurationContextProvider;
