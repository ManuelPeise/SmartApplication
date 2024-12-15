import React from "react";
import LogPage from "./LogPage";
import { LogMessage } from "../_types/logMessage";
import { useApi } from "src/_hooks/useApi";

const LogPageContainer: React.FC = (props) => {
  const { data, sendPostRequest, sendGetRequest } = useApi<LogMessage>({
    requestUrl: "Log/GetLogMessages",
    isPrivate: true,
    parameters: null,
    initialLoad: true,
  });

  const onDeleteMessages = React.useCallback(
    async (messageIds: number[]) => {
      await sendPostRequest({
        requestUrl: `Log/DeleteMessages`,
        data: JSON.stringify(messageIds),
      }).then(async () => {
        await sendGetRequest();
      });
    },
    [sendGetRequest, sendPostRequest]
  );

  return (
    <LogPage
      logMessages={(data as Array<LogMessage>) ?? []}
      onDeleteMessages={onDeleteMessages}
    />
  );
};

export default LogPageContainer;
