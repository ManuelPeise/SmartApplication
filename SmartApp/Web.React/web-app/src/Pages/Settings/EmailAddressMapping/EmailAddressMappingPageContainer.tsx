import React from "react";
import { useParams } from "react-router-dom";
import { useStatefulApiService } from "src/_hooks/useStatefulApiService";
import { EmailAccountMappings } from "../Types/EmailCleanerConfiguration";
import { StatelessApi } from "src/_lib/_api/StatelessApi";
import { useAuth } from "src/_hooks/useAuth";
import EmailAddressMappingPage from "./EmailAddressMappingPage";
import EmailAddressMappingPlaceholder from "./Components/EmailAddressMappingPlaceholder";

const EmailAddressMappingPageContainer: React.FC = () => {
  const { id } = useParams();
  const { authenticationState } = useAuth();
  const api = StatelessApi.create();

  const { isLoading, data, sendPost, rebindData } =
    useStatefulApiService<EmailAccountMappings>(
      api,
      {
        serviceUrl: `EmailCleanupSettings/GetEmailAccountMapping`,
        parameters: { accountId: id },
      },
      authenticationState.token
    );

  const initializeMappings = React.useCallback(async () => {
    await sendPost({
      serviceUrl: "EmailCleanupSettings/InitializeMappings",
      body: { accountId: id },
    }).then(async (res) => {
      if (res) {
        await rebindData();
      }
    });
  }, [id, sendPost, rebindData]);

  if (data == null) {
    return null;
  }

  if (data?.mappings == null) {
    return (
      <EmailAddressMappingPlaceholder
        allowReadEmails={data?.allowReadEmails ?? false}
        isLoading={isLoading}
        initializeMappings={initializeMappings}
      />
    );
  }

  return <EmailAddressMappingPage mappings={data} />;
};

export default EmailAddressMappingPageContainer;
