import { DropDownItem } from "src/_components/Input/Dropdown";
import { EmailProviderTypeEnum } from "src/_lib/_enums/EmailProviderTypeEnum";

export type EmailCleanerOverlayStatus = {
  addAccountOverlayOpen: boolean;
};

export type MessageLog = {
  user: string;
  timeStamp: string;
};

export type EmailAccountModel = {
  id: number;
  userId: number;
  accountName: string;
  providerType: EmailProviderTypeEnum;
  server: string;
  port: number;
  emailAddress: string;
  password: string | null;
  connectionTestPassed: boolean;
  messageLog?: MessageLog | null;
  settings: EmailCleanerSettings | null;
  emailAddressMappings: EmailAddressMappingEntry[];
};

export type EmailCleanerSettings = {
  settingsId: number;
  emailCleanerEnabled: boolean;
  emailCleanerAiEnabled: boolean;
  isAgreed: boolean;
  folderConfiguration: FolderSettings[];
  messageLog: MessageLog | null;
};

export type FolderSettings = {
  folderId: string;
  folderName: string;
  isInbox: boolean;
};

export type ConnectionTestModel = {
  server: string;
  port: number;
  emailAddress: string;
  password: string;
};

export type EmailCleanerListItem = {
  key: number;
  icon?: React.ReactElement;
  title: string;
  subTitle: string;
  disabled?: boolean;
  onClick: () => void;
};

export type AccountDetailsState = {
  mode: "view" | "edit";
  account: EmailAccountModel;
};

export type EmailAddressMappingEntry = {
  id: number;
  isActive: boolean;
  accountId: number;
  emailFolder: string;
  targetFolder: string;
  sourceAddress: string;
  subject: string;
  domain: string;
  predictedValue?: string | null;
  isSpam: boolean;
  action: number;
};

export type EmailAddressMappingFilter = {
  domain: DropDownItem;
  searchText: string;
  groupByDomain: boolean;
};
