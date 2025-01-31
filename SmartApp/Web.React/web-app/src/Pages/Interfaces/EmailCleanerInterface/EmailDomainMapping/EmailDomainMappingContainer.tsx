import React from "react";
import { useParams } from "react-router-dom";
import EmailMappingPageLayout from "../components/EmailMappingPageLayout";
import { EmailFolderMappingResponse, FolderMappingUpdate } from "../types";
import { useAuth } from "src/_hooks/useAuth";
import { StatelessApi } from "src/_lib/_api/StatelessApi";
import { useStatefulApiService } from "src/_hooks/useStatefulApiService";

const EmailDomainMappingContainer: React.FC = () => {
  const { authenticationState } = useAuth();
  const { id } = useParams();

  const api = StatelessApi.create();

  const { isLoading, data, sendPost, rebindData } =
    useStatefulApiService<EmailFolderMappingResponse>(
      api,
      {
        serviceUrl: "FolderMapping/GetFolderMappingData",
        parameters: { settingsGuid: id },
      },
      authenticationState.token
    );

  const handleUpdateMappings = React.useCallback(
    async (update: FolderMappingUpdate) => {
      await sendPost<boolean>({
        serviceUrl: "FolderMapping/UpdateFolderMappingData",
        body: update,
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
  return (
    <EmailMappingPageLayout
      mappings={data.mappingData.mappings}
      folders={data.mappingData.folders}
      settingsGuid={data.settingsGuid}
      isLoading={isLoading}
      handleUpdate={handleUpdateMappings}
    />
  );
};

export default EmailDomainMappingContainer;
