import { EmailProviderTypeEnum } from "src/_lib/_enums/EmailProviderTypeEnum";

export type EmailAccountSettings = {
  accountId: number;
  userId: number;
  accountName: string;
  providerType: EmailProviderTypeEnum;
  imapServer: string;
  imapPort: number;
  emailAddress: string;
  connectionTestPassed: boolean;
  password: string | null;
};

export type EmailAccountConnectionTestRequest = {
  accountId: number;
  server: string;
  port: number;
  emailAddress: string;
  password: string;
};

export type EmailMappingUpdateStatus = {
  open: boolean;
  success: boolean;
};
