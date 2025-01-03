import {
  EmailClassificationInformation,
  SpamClassificationEnum,
} from "./table";

export type EmailCleanupSettings = {
  id: number;
  userId: number;
  isInitialized: boolean;
  useAiPrediction: boolean;
  accountSettings: AccountSettings;
  inboxConfiguration: InboxConfiguration;
};

export type AccountSettings = {
  imapServer: string;
  port: number;
  emailAddress: string;
  password: string | null;
  connectionTestPassed: boolean;
  connectionEstablished: boolean;
};

export type InboxConfiguration = {
  messageCount: number;
  unreadMessageCount: number;
  blockListSettings: BlockListSettings;
  folderMappings: FolderMappingEntry[];
};

export type BlockListSettings = {
  backup: boolean;
  delete: boolean;
  backupFolder: string;
  blockList: string[];
};

export type FolderMappingEntry = {
  isActive: boolean;
  source: string;
  target: string;
  classificationInformations: EmailClassificationInformation[];
};

export type ImapServerConfiguration = {
  id: number;
  label: string;
  imageSrc: string;
  serverAddress: string;
  port: number;
};

export type SpamReport = {
  from: string;
  subject: string;
  classification: SpamClassificationEnum;
};
