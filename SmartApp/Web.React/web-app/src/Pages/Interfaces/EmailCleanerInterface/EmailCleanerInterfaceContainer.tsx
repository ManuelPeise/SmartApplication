import React from "react";
import { useAuth } from "src/_hooks/useAuth";
import { useStatefulApiService } from "src/_hooks/useStatefulApiService";
import { StatelessApi } from "src/_lib/_api/StatelessApi";
import {
  EmailCleanerInterfaceConfigurationUiModel,
  EmailCleanerUpdateModel,
} from "./types";
import EmailCleanerInterfacePage from "./EmailCleanerInterfacePage";
import NoDataPlaceholder from "src/_components/Placeholders/NoDataPlaceholder";
import { useI18n } from "src/_hooks/useI18n";
import { browserRoutes } from "src/_lib/Router/RouterUtils";

const EmailCleanerInterfaceContainer: React.FC = () => {
  const { authenticationState } = useAuth();
  const { getResource } = useI18n();
  const api = StatelessApi.create();

  const { isLoading, data, rebindData, sendPost } = useStatefulApiService<
    EmailCleanerInterfaceConfigurationUiModel[]
  >(
    api,
    {
      serviceUrl: "EmailCleanerInterface/GetEmailCleanerConfigurations",
      parameters: { loadFolderMappings: "false" },
    },
    authenticationState.token
  );

  const handleUpdateConfiguration = React.useCallback(
    async (model: EmailCleanerUpdateModel) => {
      await sendPost<boolean>({
        serviceUrl: "EmailCleanerInterface/UpdateEmailCleanerConfigurations",
        body: model,
      }).then(async (res) => {
        if (res) {
          await rebindData();
        }
      });
    },
    [rebindData, sendPost]
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
      handleUpdateConfiguration={handleUpdateConfiguration}
    />
  );
};

export default EmailCleanerInterfaceContainer;
