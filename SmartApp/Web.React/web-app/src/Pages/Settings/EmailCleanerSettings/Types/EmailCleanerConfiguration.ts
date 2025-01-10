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
  predictedAs: string;
};
