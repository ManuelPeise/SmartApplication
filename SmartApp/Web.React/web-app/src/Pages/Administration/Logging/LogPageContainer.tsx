import React from "react";
import LogPage from "./LogPage";
import { LogMessage } from "./Types/logMessage";
import { StatelessApi } from "src/_lib/_api/StatelessApi";
import { useStatefulApiService } from "src/_hooks/useStatefulApiService";
import { useAuth } from "src/_hooks/useAuth";

const LogPageContainer: React.FC = (props) => {
  const api = StatelessApi.create();
  const { authenticationState } = useAuth();
  const { data, sendPost, rebindData } = useStatefulApiService<LogMessage[]>(
    api,
    {
      serviceUrl: "Log/GetLogMessages",
    },
    authenticationState.token
  );

  const onDeleteMessages = React.useCallback(
    async (messageIds: number[]) => {
      await sendPost({
        serviceUrl: "Log/DeleteMessages",
        body: JSON.stringify(messageIds),
      }).then(async () => {
        await rebindData();
      });
    },
    [rebindData, sendPost]
  );

  return (
    <LogPage
      logMessages={(data as Array<LogMessage>) ?? []}
      onDeleteMessages={onDeleteMessages}
    />
  );
};

export default LogPageContainer;
