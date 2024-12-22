export enum SpamClassificationEnum {
  All = -1,
  Unknown = 0,
  Spam = 1,
  Ham = 2,
}

export type EmailCassificationFilter = {
  address: string;
  classification: SpamClassificationEnum;
};

export type AiEmailTrainingData = {
  id: number;
  from: string;
  domain: string;
  subject: string;
  classification: SpamClassificationEnum;
};
