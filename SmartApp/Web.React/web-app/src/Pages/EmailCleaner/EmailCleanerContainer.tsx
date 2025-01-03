import React from "react";
import EmailCleaner from "./EmailCleaner";
import { useApi } from "src/_hooks/useApi";
import {
  AccountSettings,
  EmailCleanupSettings,
  SpamReport,
} from "./Types/emailCleanupTypes";
import NoConnectionPlaceholder from "./NoConnectionPlaceholder";
import { SuccessResponse } from "src/_lib/_types/response";

const EmailCleanerContainer: React.FC = () => {
  const { isLoading, data, sendGetRequest, sendPost, sendPostRequest } =
    useApi<EmailCleanupSettings>({
      requestUrl: "EmailCleaner/GetEmailCleanerSettings",
      isPrivate: true,
      initialLoad: true,
      parameters: null,
    });

  const [showConnectionScreen, setShowConnectionScreen] =
    React.useState<boolean>(false);

  const handleShowConnectionScreen = React.useCallback(() => {
    setShowConnectionScreen(true);
  }, []);

  const handleHideConnectionScreen = React.useCallback(() => {
    setShowConnectionScreen(false);
  }, []);

  const handleCheckConnection = React.useCallback(
    async (settings: AccountSettings): Promise<boolean> => {
      const result = await sendPost<SuccessResponse>({
        requestUrl: "EmailCleaner/TestAccountConnection",
        data: JSON.stringify(settings),
      });

      return result.success;
    },
    [sendPost]
  );

  const handleInitializeAccountInboxSettings = React.useCallback(
    async (settings: EmailCleanupSettings): Promise<EmailCleanupSettings> => {
      const result = await sendPost<EmailCleanupSettings>({
        requestUrl: "EmailCleaner/InitializeAccountInboxSettings",
        data: JSON.stringify(settings),
      });

      return result;
    },
    [sendPost]
  );

  const handleSaveConnection = React.useCallback(
    async (settings: EmailCleanupSettings) => {
      await sendPostRequest({
        requestUrl: "EmailCleaner/AddNewAccountSettings",
        data: JSON.stringify(settings),
      }).then(async () => {
        await sendGetRequest();
        setShowConnectionScreen(false);
      });
    },
    [sendGetRequest, sendPostRequest]
  );

  const handleUpdateSettings = React.useCallback(
    async (settings: EmailCleanupSettings) => {
      await sendPostRequest({
        requestUrl: "EmailCleaner/UpdateSettings",
        data: JSON.stringify(settings),
      });
    },
    [sendPostRequest]
  );

  const handleReportAiTrainingData = React.useCallback(
    async (reportModel: SpamReport) => {
      await sendPostRequest({
        requestUrl: "EmailCleaner/ReportAiTrainingData",
        data: JSON.stringify(reportModel),
      });
    },
    [sendPostRequest]
  );

  if (!data?.length) {
    return (
      <NoConnectionPlaceholder
        isLoading={isLoading}
        showConnectionScreen={showConnectionScreen}
        handleShowConnectionScreen={handleShowConnectionScreen}
        handleHideConnectionScreen={handleHideConnectionScreen}
        handleCheckConnection={handleCheckConnection}
        handleInitializeAccountInboxSettings={
          handleInitializeAccountInboxSettings
        }
        handleSaveConnection={handleSaveConnection}
      />
    );
  }

  return (
    <EmailCleaner
      isLoading={isLoading}
      settings={data}
      showConnectionScreen={showConnectionScreen}
      handleUpdateSettings={handleUpdateSettings}
      handleReportAiTrainingData={handleReportAiTrainingData}
    ></EmailCleaner>
  );
};

export default EmailCleanerContainer;
