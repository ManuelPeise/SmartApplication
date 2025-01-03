import { Box } from "@mui/material";
import React from "react";
import { StyledListItem } from "src/_components/_styled/StyledListItem";
import {
  AccountSettings,
  EmailCleanupSettings,
  ImapServerConfiguration,
} from "../Types/emailCleanupTypes";
import { useI18n } from "src/_hooks/useI18n";
import iconNoProvider from "../img/no_Email.jpg";
import iconGmail from "../img/gmail_icon.png";
import iconGmx from "../img/gmx_icon.png";
import Dropdown from "src/_components/_input/Dropdown";

interface IProps {
  accountSettings: AccountSettings;
  handleUpdatePartial: (
    partialStateUpdate: Partial<EmailCleanupSettings>
  ) => void;
}

const ImapServerListItem: React.FC<IProps> = (props) => {
  const { accountSettings, handleUpdatePartial } = props;
  const { getResource } = useI18n();

  const availableImapServers = React.useMemo((): ImapServerConfiguration[] => {
    return [
      {
        id: 0,
        label: getResource("settings.labelNoProviderSelected"),
        imageSrc: iconNoProvider,
        serverAddress: "",
        port: 993,
      },
      {
        id: 1,
        label: getResource("settings.labelProviderGmail"),
        imageSrc: iconGmail,
        serverAddress: "imap.gmail.com",
        port: 993,
      },
      {
        id: 2,
        label: getResource("settings.labelProviderGmx"),
        imageSrc: iconGmx,
        serverAddress: "imap.gmx.net",
        port: 993,
      },
    ];
  }, [getResource]);

  return (
    <StyledListItem
      divider
      sx={{
        marginTop: 2,
        width: "100%",
        display: "flex",
        alignItems: "center",
        justifyContent: "space-between",
      }}
    >
      <Box
        component="img"
        sx={{
          padding: 1,
          height: 50,
          width: 50,
        }}
        alt="email provider logo"
        src={
          availableImapServers.find(
            (server) => server.serverAddress === accountSettings.imapServer
          )?.imageSrc ?? availableImapServers[0].imageSrc
        }
      />
      <Box width="30%">
        <Dropdown
          fullWidth
          items={availableImapServers.map((server) => {
            return {
              key: server.id,
              label: server.label,
              disabled:
                availableImapServers.find(
                  (server) =>
                    server.serverAddress === accountSettings.imapServer
                )?.id === server.id,
            };
          })}
          value={
            availableImapServers.find(
              (server) => server.serverAddress === accountSettings.imapServer
            )?.id ?? 0
          }
          onChange={(value) => {
            const serverSettings =
              availableImapServers.find((server) => server.id === value) ??
              null;

            if (serverSettings != null) {
              handleUpdatePartial({
                accountSettings: {
                  ...accountSettings,
                  imapServer: serverSettings.serverAddress,
                  port: serverSettings.port,
                },
              });
            }
          }}
        />
      </Box>
    </StyledListItem>
  );
};

export default ImapServerListItem;
