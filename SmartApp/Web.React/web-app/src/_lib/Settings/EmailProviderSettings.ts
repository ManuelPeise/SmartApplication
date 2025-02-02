import { EmailProviderTypeEnum } from "../_enums/EmailProviderTypeEnum";
import noEmailProviderImg from "../_img/noEmailProvider.png";
import gmailEmailProviderImg from "../_img/gmail_icon.png";
import gmxEmailProviderImg from "../_img/gmx_icon.png";

export type EmailProviderSettings = {
  type: EmailProviderTypeEnum;
  resourceKey: string;
  imageSrc: string;
  server: string;
  port: number;
};

export const emailProviderSettings: EmailProviderSettings[] = [
  {
    type: EmailProviderTypeEnum.None,
    resourceKey: "settings.labelNoEmailProviderSelected",
    imageSrc: noEmailProviderImg,
    server: "",
    port: 993,
  },
  {
    type: EmailProviderTypeEnum.Gmx,
    resourceKey: "settings.labelProviderGmx",
    imageSrc: gmxEmailProviderImg,
    server: "imap.gmx.net",
    port: 993,
  },
  {
    type: EmailProviderTypeEnum.Gmail,
    resourceKey: "settings.labelProviderGmail",
    imageSrc: gmailEmailProviderImg,
    server: "imap.gmail.com",
    port: 993,
  },
];

export const getEmailProviderSettings = (type: EmailProviderTypeEnum) => {
  return (
    emailProviderSettings.find((x) => x.type === type) ?? {
      type: EmailProviderTypeEnum.None,
      resourceKey: "settings.labelNoEmailProviderSelected",
      imageSrc: noEmailProviderImg,
    }
  );
};
