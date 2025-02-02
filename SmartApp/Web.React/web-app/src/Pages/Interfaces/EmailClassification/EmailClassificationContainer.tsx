import React from "react";
import { useParams } from "react-router-dom";
import { useAuth } from "src/_hooks/useAuth";
import { useStatefulApiService } from "src/_hooks/useStatefulApiService";
import { StatelessApi } from "src/_lib/_api/StatelessApi";
import {
  EmailClassificationModel,
  EmailClassificationPageModel,
} from "./types";
import EmailClassificationPage from "./components/EmailClassificationPage";

const EmailClassificationContainer: React.FC = () => {
  const { authenticationState } = useAuth();
  const { id } = useParams();

  const api = StatelessApi.create();

  const { data, sendPost, rebindData } =
    useStatefulApiService<EmailClassificationPageModel>(
      api,
      {
        serviceUrl: "EmailClassification/GetSpamClassificationData",
        parameters: { accountId: id },
      },
      authenticationState.token
    );

  const handleSave = React.useCallback(
    async (items: EmailClassificationModel[]) => {
      await sendPost<boolean>({
        serviceUrl: "EmailClassification/UpdateSpamClassificationData",
        body: items,
      }).then(async (res) => {
        if (res) {
          await rebindData();
        }
      });
    },
    [rebindData, sendPost]
  );

  if (data == null || !data?.classificationModels?.length) {
    return null;
  }

  return (
    <EmailClassificationPage
      classifications={data.classificationModels}
      folders={data.folders}
      folderPredictionEnabled={data.folderPredictionEnabled}
      spamPredictionEnabled={data.spamPredictionEnabled}
      handleSave={handleSave}
    />
  );
};

export default EmailClassificationContainer;
