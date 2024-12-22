import React from "react";
import { useApi } from "src/_hooks/useApi";
import { AiEmailTrainingData } from "./spamTypes";
import SpamMailClassificationPage from "./SpamMailClassificationPage";

const SpamMailClassificationContainer: React.FC = () => {
  const { data, isLoading, sendGetRequest, sendPost } =
    useApi<AiEmailTrainingData>({
      requestUrl: "EmailClassification/GetAiEmailTrainingData",
      isPrivate: true,
      parameters: null,
      initialLoad: true,
    });

  const handleSave = React.useCallback(
    async (items: AiEmailTrainingData[]) => {
      await sendPost({
        requestUrl: "EmailClassification/UpdateAiEmailTrainingData",
        data: JSON.stringify(items),
      }).then(async () => {
        await sendGetRequest();
      });
    },
    [sendGetRequest, sendPost]
  );

  if (data == null) {
    return null;
  }

  return (
    <SpamMailClassificationPage
      originalData={data}
      isLoading={isLoading}
      handleSave={handleSave}
    />
  );
};

export default SpamMailClassificationContainer;
