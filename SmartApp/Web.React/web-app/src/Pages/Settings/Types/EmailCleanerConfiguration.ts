export type EmailCleanerConfiguration = {
  userId: number;
  accounts: EmailCleanerAccount[];
  settings: EmailCleanerSettings[];
};

export type EmailCleanerAccount = {
  id: number;
  accountName: string;
};

export type EmailCleanerSettings = {
  settingsId: number;
  accountId: number;
  enabled: boolean;
  allowReadEmails: boolean;
  allowMoveEmails: boolean;
  allowDeleteEmails: boolean;
  allowCreateEmailFolder: boolean;
  shareEmailDataToTrainAi: boolean;
  scheduleCleanup: boolean;
  scheduleCleanupAtHour: number;
  hasMappings: boolean;
  lastCleanupTime: string;
  mappings: EmailAddressMapping[];
};

export type EmailAddressMapping = {
  sourceAddress: string;
  domain: string;
  shouldCleanup: boolean;
  isSpam: boolean;
  predictedValue: string;
  action: string; // define enum for cleanup actions, add column for predict value by ai (OnlinePredictionIcon)
};

export type EmailAccountMappings = {
  settingsId: number;
  userId: number;
  allowReadEmails: boolean;
  mappings: EmailAddressMapping[] | null;
};

export type EmailAddressMappingFilter = {
  domainIndex: number;
  searchText: string;
};

export type EmailTableColumnDefinition = {
  id:
    | "sourceAddress"
    | "domain"
    | "predictedValue"
    | "isSpam"
    | "shouldCleanup";
  recourceKey: string;
  width?: number;
  minWidth?: number;
  maxWidth?: number;
  align?: "left" | "right" | "center";
};
