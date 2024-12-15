import { EmailActionEnum } from "../_enums/EmailActionEnum";

export type EmailAccountSettings = {
  emailServerAddress?: string;
  port?: number;
  emailAddress?: string;
  password?: string;
  emailMappings: EmailAddressMapping;
};

export type EmailAddressMapping = {
  id: number;
  address: string;
  action: EmailActionEnum;
};
