import React from "react";
import { useAuth } from "src/_hooks/useAuth";
import { useStatefulApiService } from "src/_hooks/useStatefulApiService";
import { StatelessApi } from "src/_lib/_api/StatelessApi";
import { EmailCleanerSettings } from "./types";
import EmailCleanerInterfacePage from "./EmailCleanerInterfacePage";
import NoDataPlaceholder from "src/_components/Placeholders/NoDataPlaceholder";
import { useI18n } from "src/_hooks/useI18n";
import { browserRoutes } from "src/_lib/Router/RouterUtils";

const EmailCleanerInterfaceContainer: React.FC = () => {
  const { authenticationState } = useAuth();
  const { getResource } = useI18n();
  const api = StatelessApi.create();

  const { isLoading, data, rebindData, sendPost } = useStatefulApiService<
    EmailCleanerSettings[]
  >(
    api,
    {
      serviceUrl: "EmailCleanerInterface/GetEmailCleanerSettings",
    },
    authenticationState.token
  );

  const handleUpdateSettings = React.useCallback(
    async (model: EmailCleanerSettings) => {
      await sendPost<boolean>({
        serviceUrl: "EmailCleanerInterface/UpdateEmailCleanerSettings",
        body: model,
      }).then(async (res) => {
        if (res) {
          await rebindData();
        }
      });
    },
    [rebindData, sendPost]
  );

  const handleImportData = React.useCallback(
    async (accountId: number) => {
      await sendPost({
        serviceUrl: "EmailCleanerInterface/ExecuteEmailDataImport",
        parameters: { accountId: accountId.toFixed(0) },
      });
    },
    [sendPost]
  );

  if (!data) {
    return null;
  }

  if (data && !data.length) {
    return (
      <NoDataPlaceholder
        buttonLabel={getResource("interface.labelConfigure")}
        infoText={getResource("common.labelNoDataMessage").replace(
          "{MessageExtension}",
          getResource("interface.labelConfigureEmailAccount")
        )}
        route={browserRoutes.emailAccountInterface}
      />
    );
  }

  return (
    <EmailCleanerInterfacePage
      isLoading={isLoading}
      data={data}
      handleUpdateSettings={handleUpdateSettings}
      handleImportData={handleImportData}
    />
  );
};

export default EmailCleanerInterfaceContainer;
