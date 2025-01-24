export type SpamClassificationPageData = {
  statistics: SpamPredictionStatisticData;
  domains: EmailDomainModel[];
};

export type SpamPredictionStatisticData = {
  trainingEntityCount: number;
  trainingFileTimeStamp: string;
  modelsFileTimeStamp: string;
  averageEntrophy: number;
  metrics;
};

export type SpamPredictionMetric = {
  timeStamp: string;
  accuracy: number;
  f1Score: number;
  entropy: number;
  logLoss: number;
  logLossReduction: number;
};

export type EmailDomainModel = {
  id: number;
  domainName: string;
  classificationDataSets: SpamClassificationDataSet[];
};

export type SpamClassificationDataSet = {
  id: number;
  email: string;
  subject: string;
  classification: number;
};

export type SpamClassificationColumn = {
  key: keyof SpamClassificationDataSet;
  label: string;
  minWidth: number;
  align: "left" | "center" | "right";
  renderComponent: (
    props: SpamClassificationDataSet,
    rowId?: number
  ) => React.ReactNode;
};

export type SaveTrainingDataRequest = {
  models: SpamClassificationDataSet[];
};

export type PredictionRequest = {
  emailAddress: string;
  subject: string;
};

export type EmailPrediction = {
  request: PredictionRequest;
  label?: boolean;
  probability: number;
  score: number;
};
