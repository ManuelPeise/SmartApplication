import { EmailProviderTypeEnum } from "src/_lib/_enums/EmailProviderTypeEnum";

export type EmailCleanerInterfaceConfigurationUiModel = {
  settingsGuid: string;
  userId: number;
  accountName: string;
  emailAddress: string;
  providerType: EmailProviderTypeEnum;
  unmappedDomains: number;
  connectionTestPassed: boolean;
  emailCleanerEnabled: boolean;
  useAiSpamPrediction: boolean;
  useAiTargetFolderPrediction: boolean;
  updatedBy?: string;
  updatedAt?: string;
  domainFolderMapping: EmailDomainFolderMappingUiModel[];
};

export type EmailDomainFolderMappingUiModel = {
  isActive: boolean;
  sourceDomain: string;
  targetFolder: string;
  predictedTargetFolder: sting;
  automatedCleanupEnabled: boolean;
  forceDeleteSpamMails: boolean;
  currentEmailData: EmailDomainModel[];
};

export type EmailDomainModel = {
  emailId: string;
  sourceAddress: string;
  domain: string;
  subject: string;
  predictionResult: string;
  isSpam: boolean;
  isNew: boolean;
};

export type EmailCleanerUpdateModel = {
  settingsGuid: string;
  emailCleanerEnabled: boolean;
  useAiSpamPrediction: boolean;
  useAiTargetFolderPrediction: boolean;
};
