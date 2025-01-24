import { Box } from "@mui/material";
import React from "react";
import {
  EmailDomainModel,
  EmailPrediction,
  PredictionRequest,
  SaveTrainingDataRequest,
  SpamPredictionStatisticData,
} from "./types";
import EmailClassificationInfoTab from "./EmailClassificationInfoTab";
import EmailClassificationDefinitionTab from "./EmailClassificationDefinitionTab";

interface IProps {
  selectedTab: number;
  domainData: EmailDomainModel[];
  statistics: SpamPredictionStatisticData;
  handleModifiedState: React.Dispatch<React.SetStateAction<boolean>>;
  handleTrainModel: () => Promise<void>;
  saveTrainingData: (request: SaveTrainingDataRequest) => Promise<boolean>;
  handlePredictEmail: (model: PredictionRequest) => Promise<EmailPrediction>;
}

const SpamClassificationPageBody: React.FC<IProps> = (props) => {
  const {
    selectedTab,
    domainData,
    statistics,
    handleModifiedState,
    saveTrainingData,
    handleTrainModel,
    handlePredictEmail,
  } = props;

  return (
    <Box
      width="100%"
      height="100%"
      display="flex"
      flexDirection="column"
      justifyContent="center"
      gap={2}
      padding={2}
    >
      <EmailClassificationInfoTab
        tabIndex={0}
        selectedTab={selectedTab}
        statistics={statistics}
        handleTrainModel={handleTrainModel}
        handlePredictEmail={handlePredictEmail}
      />
      <EmailClassificationDefinitionTab
        tabIndex={1}
        selectedTab={selectedTab}
        domains={domainData}
        handleModifiedState={handleModifiedState}
        saveTrainingData={saveTrainingData}
      />
    </Box>
  );
};

export default SpamClassificationPageBody;
