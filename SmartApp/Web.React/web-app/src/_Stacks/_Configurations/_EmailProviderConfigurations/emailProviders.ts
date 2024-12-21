import { EmailProviderTypeEnum } from "./_enums/EmailProviderTypeEnum";
import noEmailLogo from "./img/no_Email.jpg";
import gmailLogo from "./img/gmail_icon.png";
import gmxLogo from "./img/gmx_icon.png";
import { EmailProvider, EmailProviderConfiguration } from "./types";
import { EmailProviderConfigurationStateEnum } from "./_enums/EmailProviderConfigurationStateEnum";

export const emailProviders: EmailProvider[] = [
  {
    providerType: EmailProviderTypeEnum.None,
    imapServerAddress: "",
    imapPort: null,
    logo: noEmailLogo,
    displayName: "None",
  },
  {
    providerType: EmailProviderTypeEnum.GoogleMail,
    imapServerAddress: "imap.gmail.com",
    imapPort: 993,
    logo: gmailLogo,
    displayName: "Google Mail",
  },
  {
    providerType: EmailProviderTypeEnum.Gmx,
    imapServerAddress: "imap.gmx.net",
    imapPort: 993,
    logo: gmxLogo,
    displayName: "Gmx",
  },
];

export const getDefaultEmailProviderConfiguration = (
  id: number,
  label?: string
): EmailProviderConfiguration => {
  return {
    id: id,
    name: label,
    emailAddress: "",
    password: "",
    provider: emailProviders.find(
      (x) => x.providerType === EmailProviderTypeEnum.None
    ),
    status: EmailProviderConfigurationStateEnum.Pending,
    connectionTestPasses: false,
    // isValid: false,
    connectionInfo: null,
  };
};
