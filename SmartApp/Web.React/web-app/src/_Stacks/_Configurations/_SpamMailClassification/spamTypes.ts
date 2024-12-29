export enum SpamClassificationEnum {
  All = -1,
  Spam = 0,
  Ham = 1,
  Unknown = 2,
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
