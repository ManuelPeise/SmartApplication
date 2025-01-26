import { EmailProviderTypeEnum } from "src/_lib/_enums/EmailProviderTypeEnum";

export type EmailAccountSettings = {
  settingsGuid: string;
  userId: number;
  accountName: string;
  providerType: EmailProviderTypeEnum;
  server: string;
  port: number;
  emailAddress: string;
  connectionTestPassed: boolean;
  password: string | null;
};

export type EmailAccountConnectionTestRequest = {
  server: string;
  port: number;
  emailAddress: string;
  password: string;
};

export type EmailMappingUpdateStatus = {
  open: boolean;
  success: boolean;
};
