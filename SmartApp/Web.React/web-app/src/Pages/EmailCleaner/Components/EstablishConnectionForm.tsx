import { ClearOutlined } from "@material-ui/icons";
import { Box, IconButton, List, Paper, Typography } from "@mui/material";
import React from "react";
import { useFormState } from "src/_hooks/useFormState";
import {
  AccountSettings,
  EmailCleanupSettings,
} from "../Types/emailCleanupTypes";
import ImapServerListItem from "./ImapServerListItem";
import ListItemInput from "src/_components/_list/ListItemInput";
import { useI18n } from "src/_hooks/useI18n";
import TextInput from "src/_components/_input/TextInput";
import FormButton from "src/_components/_buttons/FormButton";
import { isEqual } from "lodash";
import LoadingIndicator from "src/_components/_loading/LoadingIndicator";

interface IProps {
  isLoading: boolean;
  handleHideConnectionScreen: () => void;
  handleCheckConnection: (settings: AccountSettings) => Promise<boolean>;
  handleInitializeAccountInboxSettings: (
    settings: EmailCleanupSettings
  ) => Promise<EmailCleanupSettings>;
  handleSaveConnection: (settings: EmailCleanupSettings) => Promise<void>;
}

const defaultEmailCleanupSettings: EmailCleanupSettings = {
  id: 0,
  userId: 0,
  isInitialized: false,
  useAiPrediction: false,
  accountSettings: {
    imapServer: "",
    port: 993,
    emailAddress: "",
    password: "",
    connectionEstablished: false,
    connectionTestPassed: false,
  },
  inboxConfiguration: {
    messageCount: 0,
    unreadMessageCount: 0,
    folderMappings: [],
    blockListSettings: {
      delete: false,
      backup: false,
      backupFolder: "",
      blockList: [],
    },
  },
};

const EstablishConnectionForm: React.FC<IProps> = (props) => {
  const {
    isLoading,
    handleHideConnectionScreen,
    handleCheckConnection,
    handleInitializeAccountInboxSettings,
    handleSaveConnection,
  } = props;
  const { getResource } = useI18n();
  const form = useFormState<EmailCleanupSettings>(defaultEmailCleanupSettings);

  const { formState } = React.useMemo(() => {
    return form.subScribe();
  }, [form]);

  const handleConnectionCheck = React.useCallback(async () => {
    const checkPassed = await handleCheckConnection(formState.accountSettings);

    form.handleUpdatePartial({
      accountSettings: {
        ...formState.accountSettings,
        connectionTestPassed: checkPassed,
      },
    });
  }, [formState, form, handleCheckConnection]);

  const onInitializeSettings = React.useCallback(async () => {
    await handleInitializeAccountInboxSettings(formState).then((res) => {
      form.handleUpdate(res);
    });
  }, [form, formState, handleInitializeAccountInboxSettings]);

  const handleSave = React.useCallback(async () => {
    await handleSaveConnection(formState);
  }, [formState, handleSaveConnection]);

  return (
    <Paper
      sx={{
        display: "flex",
        flexDirection: "column",
        justifyContent: "space-between",
        alignItems: "center",
        height: "80%",
        maxHeight: "80%",
        overflowX: "scroll",
        width: "60%",
        opacity: 0.8,
        padding: 2,
      }}
    >
      <Box width="100%">
        <Box
          width="100%"
          display="flex"
          flexDirection="column"
          justifyContent="space-between"
        >
          <Box
            width="100%"
            display="flex"
            alignItems="baseline"
            justifyContent="space-between"
          >
            <Typography sx={{ marginLeft: "2rem" }} variant="h6">
              {getResource("settings.labelEstablishEmailAccountConnection")}
            </Typography>
            <IconButton size="small" onClick={handleHideConnectionScreen}>
              <ClearOutlined />
            </IconButton>
          </Box>
          <LoadingIndicator isLoading={isLoading} />
        </Box>
        <Box width="100%">
          <List sx={{ padding: 2 }}>
            <ImapServerListItem
              key="imap-server-selection"
              accountSettings={formState?.accountSettings}
              handleUpdatePartial={form.handleUpdatePartial}
            />
            <ListItemInput
              key="email-address-input-field"
              childWidth="12rem"
              description={getResource("settings.descriptionEmailAddress")}
            >
              <TextInput
                value={formState.accountSettings.emailAddress}
                fullwidth
                label={getResource("settings.labelEmailAddress")}
                disabled={formState.accountSettings.imapServer === ""}
                onChange={(value) =>
                  form.handleUpdatePartial({
                    accountSettings: {
                      ...formState.accountSettings,
                      emailAddress: value,
                    },
                  })
                }
              />
            </ListItemInput>
            <ListItemInput
              key="password-input-field"
              childWidth="12rem"
              description={getResource("settings.descriptionEmailPassword")}
            >
              <TextInput
                value={formState.accountSettings.password}
                fullwidth
                isPassword
                label={getResource("settings.labelEmailPassword")}
                disabled={formState.accountSettings.imapServer === ""}
                onChange={(value) =>
                  form.handleUpdatePartial({
                    accountSettings: {
                      ...formState.accountSettings,
                      password: value,
                    },
                  })
                }
              />
            </ListItemInput>
            <ListItemInput
              key="connection-test-button"
              childWidth="12rem"
              description={getResource(
                "settings.descriptionEmailAccountConnectionCheck"
              )}
            >
              <FormButton
                label={
                  !formState.accountSettings.connectionTestPassed
                    ? getResource("settings.labelCheck")
                    : getResource("settings.labelCheckPassed")
                }
                disabled={
                  formState.accountSettings.emailAddress === "" ||
                  formState.accountSettings.password === "" ||
                  formState.accountSettings.connectionTestPassed
                }
                onAction={handleConnectionCheck}
              />
            </ListItemInput>
            <ListItemInput
              key="initialize-settings-button"
              childWidth="12rem"
              description={getResource(
                "settings.descriptionInitializeAccountSettings"
              )}
            >
              <FormButton
                label={getResource("settings.labelInitializeSettings")}
                disabled={
                  !formState.accountSettings.connectionTestPassed ||
                  formState.isInitialized
                }
                onAction={onInitializeSettings}
              />
            </ListItemInput>
          </List>
        </Box>
      </Box>
      <Box width="100%" display="flex" flexDirection="row">
        <Box width="100%" display="flex" justifyContent="flex-end" gap={3}>
          <FormButton
            label={getResource("settings.labelCancel")}
            disabled={isEqual(defaultEmailCleanupSettings, formState)}
            onAction={form.handleReset}
          />
          <FormButton
            label={getResource("settings.labelSave")}
            disabled={
              !formState.accountSettings.connectionTestPassed ||
              !formState.isInitialized
            }
            onAction={handleSave}
          />
        </Box>
      </Box>
    </Paper>
  );
};

export default EstablishConnectionForm;
