export type InboxTableColumnDefinition = {
  id: "date" | "from" | "subject" | "ai-classification" | "action";
  label: string;
  width?: number | "auto";
  align: "right" | "left" | "center";
  formatValue?: (value: number) => string;
};

export enum SpamClassificationEnum {
  Spam = 0,
  Ham = 1,
  Unknown = 2,
}

export type SpamClassification = {
  classification: SpamClassificationEnum;
  score: string;
};

export type EmailClassificationInformation = {
  id: string;
  from: string;
  subject: string;
  messageDate: string;
  classification: SpamClassification;
};

export type TableDataRow = {
  from: string;
  subject: string;
  classification: SpamClassification;
};
