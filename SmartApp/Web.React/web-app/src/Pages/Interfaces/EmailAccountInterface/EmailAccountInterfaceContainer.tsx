import React from "react";
import { useAuth } from "src/_hooks/useAuth";
import { useStatefulApiService } from "src/_hooks/useStatefulApiService";
import { StatelessApi } from "src/_lib/_api/StatelessApi";
import {
  EmailAccountConnectionTestRequest,
  EmailAccountSettings,
} from "./types";
import EmailAccountInterface from "./EmailAccountInterface";

const EmailAccountInterfaceContainer: React.FC = () => {
  const { authenticationState } = useAuth();
  const api = StatelessApi.create();

  const { isLoading, data, sendPost, rebindData } = useStatefulApiService<
    EmailAccountSettings[]
  >(
    api,
    { serviceUrl: "EmailAccountInterface/GetEmailAccountSettings" },
    authenticationState.token
  );

  const handleTestConnection = React.useCallback(
    async (request: EmailAccountConnectionTestRequest): Promise<boolean> => {
      return sendPost<boolean>({
        serviceUrl: "EmailAccountInterface/TestConnection",
        body: request,
      });
    },
    [sendPost]
  );

  const handleSaveConnection = React.useCallback(
    async (connection: EmailAccountSettings) => {
      await sendPost<boolean>({
        serviceUrl: "EmailAccountInterface/UpdateEmailAccountSettings",
        body: connection,
      }).then(async (res) => {
        if (res) {
          await rebindData();
        }
      });
    },
    [rebindData, sendPost]
  );

  if (!data?.length) {
    return null;
  }

  return (
    <EmailAccountInterface
      isLoading={isLoading}
      data={data}
      handleTestConnection={handleTestConnection}
      handleSaveConnection={handleSaveConnection}
    />
  );
};

export default EmailAccountInterfaceContainer;
