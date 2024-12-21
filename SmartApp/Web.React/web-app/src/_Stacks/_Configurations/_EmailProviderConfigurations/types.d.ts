import moment from "moment";
import { EmailProviderTypeEnum } from "./_enums/EmailProviderTypeEnum";
import { EmailProviderConfigurationStateEnum } from "./_enums/EmailProviderConfigurationStateEnum";

export type EmailProvider = {
  providerType: EmailProviderTypeEnum;
  imapServerAddress: string;
  imapPort: number | null;
  logo: string;
  displayName: string;
};

export type EmailProviderConnectionInfo = {
  updatedAt: moment.Moment;
  updatedBy: string;
};

export type EmailProviderConfiguration = {
  id: number;
  name: string;
  provider: EmailProvider;
  emailAddress?: string;
  password?: string;
  status: EmailProviderConfigurationStateEnum;
  connectionTestPasses: boolean;
  allowCollectAiTrainingData: boolean;
  connectionInfo: EmailProviderConnectionInfo | null;
};
