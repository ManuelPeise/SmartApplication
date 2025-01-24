import React from "react";
import {
  EmailPrediction,
  PredictionRequest,
  SaveTrainingDataRequest,
  SpamClassificationPageData,
} from "./types";
import SpamClassificationPage from "./SpamClassificationPage";
import { useAuth } from "src/_hooks/useAuth";
import { StatelessApi } from "src/_lib/_api/StatelessApi";
import { useStatefulApiService } from "src/_hooks/useStatefulApiService";

const SpamClassificationContainer: React.FC = () => {
  const { authenticationState } = useAuth();
  const api = StatelessApi.create();

  const { isLoading, data, sendPost, rebindData } =
    useStatefulApiService<SpamClassificationPageData>(
      api,
      {
        serviceUrl: "SpamClassificationTraining/GetSpamClassificationPageData",
      },
      authenticationState.token
    );

  const saveTrainingData = React.useCallback(
    async (request: SaveTrainingDataRequest): Promise<boolean> => {
      let result = false;
      await sendPost<boolean>({
        serviceUrl: "SpamClassificationTraining/SaveTrainingData",
        body: request,
      }).then(async (res) => {
        if (res) {
          result = res;
          await rebindData();
        }
      });

      return result;
    },
    [rebindData, sendPost]
  );

  const handleTrainModel = React.useCallback(async (): Promise<void> => {
    await sendPost<void>({
      serviceUrl: "SpamClassificationTraining/Train",
    }).then(async () => {
      await rebindData();
    });
  }, [rebindData, sendPost]);

  const handlePredictEmail = React.useCallback(
    async (model: PredictionRequest): Promise<EmailPrediction> => {
      return await sendPost<EmailPrediction>({
        serviceUrl: "SpamClassificationTraining/Predict",
        body: model,
      }).then((res) => {
        return res;
      });
    },
    [sendPost]
  );

  if (!data) {
    return null;
  }

  return (
    <SpamClassificationPage
      isLoading={isLoading}
      pageData={data}
      saveTrainingData={saveTrainingData}
      handleTrainModel={handleTrainModel}
      handlePredictEmail={handlePredictEmail}
    />
  );
};

export default SpamClassificationContainer;
