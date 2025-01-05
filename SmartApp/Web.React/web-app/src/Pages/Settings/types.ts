import { EmailProviderTypeEnum } from "src/_lib/_enums/EmailProviderTypeEnum";
import { AuthenticationState } from "src/_lib/_types/auth";

export type EmailAccountInitializationSettingsProps = {
  items: EmailAccountSettingsModel[];
  onSave: (
    settings: EmailAccountSettingsModel
  ) => Promise<EmailAccountSettingsModel>;
};

export type EmailAccountSettingsProps =
  EmailAccountInitializationSettingsProps & {
    authenticationState: AuthenticationState;
    handleAddAccount: (
      account: EmailAccountSettingsModel,
      initModel: EmailAccountSettingsModel,
      index: number
    ) => void;
  };

export type EmailAccountSettingsModel = {
  id: number;
  userId: number;
  accountName: string;
  providerType: EmailProviderTypeEnum;
  server: string;
  port: number;
  emailAddress: string;
  password: string | null;
  messageLog?: MessageLog | null;
};

export type MessageLog = {
  user: string;
  timeStamp: string;
};

export type EmailConnectionOverlayState = {
  connectionOverlayOpen: boolean;
  mode: "view" | "add" | "update";
};
