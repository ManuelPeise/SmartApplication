import { Box, Grid2, IconButton, Tooltip, Typography } from "@mui/material";
import React from "react";
import DetailsView, { ButtonProps } from "src/_components/Layouts/DetailsView";
import LoadingIndicator from "src/_components/Loading/LoadingIndicator";
import { emailProviderSettings } from "src/_lib/Settings/EmailProviderSettings";
import { EmailCleanerSettings, FolderSettings } from "../EmailCleanerTypes";
import { EmailProviderTypeEnum } from "src/_lib/_enums/EmailProviderTypeEnum";
import ListItemInput from "src/_components/Lists/ListItemInput";
import SwitchInput from "src/_components/Input/SwitchInput";
import { useI18n } from "src/_hooks/useI18n";
import { useAgreementDialog } from "src/_hooks/useAgreementDialog";
import FormButton from "src/_components/Buttons/FormButton";
import AgreementDialog from "src/_components/Dialogs/AgreementDialog";
import { isEqual } from "lodash";
import { MoreVertRounded } from "@mui/icons-material";
import EmailCleanerOverlay from "./EmailCleanerOverlay";
import InboxFolderConfiguration from "./InboxFolderConfiguration";

interface IProps {
  accountId: number;
  isLoading: boolean;
  emailAddress: string;
  providerType: EmailProviderTypeEnum;
  settings?: EmailCleanerSettings;
  handleUpdateSettings: (model: EmailCleanerSettings) => Promise<void>;
  handleGetUpdatedFolders: (accountId: number) => Promise<FolderSettings[]>;
}

const EmailCleanerSettingsForm: React.FC<IProps> = (props) => {
  const {
    accountId,
    isLoading,
    emailAddress,
    providerType,
    settings,
    handleUpdateSettings,
    handleGetUpdatedFolders,
  } = props;
  const { getResource } = useI18n();

  const [intermediateSettings, setIntermediateSettings] =
    React.useState<EmailCleanerSettings>(settings);

  const [folderConfigurationOpen, setFolderConfigurationOpen] =
    React.useState<boolean>(false);

  React.useEffect(() => {
    if (settings) {
      setIntermediateSettings(settings);
    }
  }, [settings]);

  const handleSettingsChanged = React.useCallback(
    (partialState: Partial<EmailCleanerSettings>) => {
      setIntermediateSettings({
        ...intermediateSettings,
        ...partialState,
      });
    },
    [intermediateSettings]
  );

  const setAggreed = React.useCallback(
    (state: boolean) => {
      handleSettingsChanged({ isAgreed: state });
    },
    [handleSettingsChanged]
  );

  const handleCancel = React.useCallback(() => {
    setIntermediateSettings(settings);
  }, [settings]);

  const { open, agreementText, setOpen, onAggree } = useAgreementDialog(
    intermediateSettings?.isAgreed ?? false,
    getResource("emailCleaner.agreementTextGeneralEmailCleanerSettings")
      .replace("{MYAPP}", process.env.REACT_APP_Name)
      .replace("{EMAILADDRESS}", emailAddress),
    setAggreed
  );

  const { hasFolderConfigurationError, errorMsg } = React.useMemo(() => {
    const hasFolderConfigurationError =
      !intermediateSettings.folderConfiguration.some((x) => x.isInbox);
    const errorMsg = hasFolderConfigurationError
      ? getResource("emailCleaner.lableFolderConfigurationError")
      : "";
    return { hasFolderConfigurationError, errorMsg };
  }, [intermediateSettings.folderConfiguration, getResource]);

  const providerSettings = React.useMemo(() => {
    const type = providerType ?? EmailProviderTypeEnum.None;
    return emailProviderSettings.find((x) => x.type === type);
  }, [providerType]);

  const isModified = React.useMemo(() => {
    if (!settings) {
      return false;
    }
    return !isEqual(settings, intermediateSettings);
  }, [settings, intermediateSettings]);

  const isValid = React.useMemo(() => {
    return (
      (intermediateSettings.emailCleanerEnabled &&
        intermediateSettings.isAgreed &&
        intermediateSettings.folderConfiguration.filter((x) => x.isInbox)
          .length) ||
      !intermediateSettings.emailCleanerEnabled
    );
  }, [intermediateSettings]);

  const saveCancelButtonProps = React.useMemo((): ButtonProps[] => {
    return [
      {
        label: getResource("common.labelCancel"),
        disabled: !isModified,
        onAction: handleCancel,
      },
      {
        label: getResource("common.labelSave"),
        disabled: !isModified || !isValid,
        onAction: handleUpdateSettings.bind(null, intermediateSettings),
      },
    ];
  }, [
    intermediateSettings,
    isModified,
    isValid,
    getResource,
    handleCancel,
    handleUpdateSettings,
  ]);

  if (!settings) {
    return null;
  }

  return (
    <DetailsView
      saveCancelButtonProps={saveCancelButtonProps}
      additionalButtonProps={[]}
    >
      <Grid2>
        <LoadingIndicator isLoading={isLoading} />
        <Grid2 id="email-cleaner-settings-form" width="100%" size={12} p={1}>
          <Grid2 container width="100%" spacing={1} pt={1} size={12}>
            <Grid2
              paddingLeft={2}
              size={12}
              display="flex"
              justifyContent="flex-start"
              alignItems="center"
            >
              <Box
                component="img"
                src={providerSettings.imageSrc}
                alt="provider logo"
                sx={{ paddingLeft: 1, width: "50px", height: "50px" }}
              />
            </Grid2>
            <Grid2 container size={12} spacing={1} paddingTop={0}>
              <ListItemInput
                label={getResource(
                  "emailCleaner.descriptionEmailCleanerEnabled"
                )}
              >
                <SwitchInput
                  checked={intermediateSettings?.emailCleanerEnabled}
                  handleChange={(e) =>
                    handleSettingsChanged({
                      emailCleanerEnabled: e.currentTarget.checked,
                      emailCleanerAiEnabled: !e.currentTarget.checked
                        ? false
                        : intermediateSettings?.emailCleanerAiEnabled,
                      isAgreed: false,
                    })
                  }
                />
              </ListItemInput>
            </Grid2>
            <Grid2 container size={12} spacing={1} paddingTop={0}>
              <ListItemInput
                label={getResource(
                  "emailCleaner.descriptionEmailCleanerAiEnabled"
                )}
              >
                <SwitchInput
                  checked={intermediateSettings?.emailCleanerAiEnabled}
                  handleChange={(e) =>
                    handleSettingsChanged({
                      emailCleanerAiEnabled: e.currentTarget.checked,
                      isAgreed: !e.currentTarget.checked
                        ? false
                        : intermediateSettings?.isAgreed,
                    })
                  }
                />
              </ListItemInput>
            </Grid2>
            <Grid2 container size={12} spacing={1} paddingTop={0}>
              <ListItemInput
                label={
                  intermediateSettings?.isAgreed && settings.messageLog
                    ? getResource("emailCleaner.descriptionFolderConfiguration")
                        .replace("{USER}", intermediateSettings.messageLog.user)
                        .replace(
                          "{AT}",
                          intermediateSettings.messageLog.timeStamp
                        )
                    : getResource("emailCleaner.descriptionNoFoldersConfigured")
                }
              >
                <Tooltip
                  title={errorMsg}
                  children={
                    <IconButton
                      sx={{
                        border: hasFolderConfigurationError
                          ? "1px solid red"
                          : "1px solid transparent",
                      }}
                      size="medium"
                      onClick={setFolderConfigurationOpen.bind(null, true)}
                    >
                      <MoreVertRounded />
                    </IconButton>
                  }
                />
              </ListItemInput>
            </Grid2>
            <Grid2 container size={12} spacing={1} paddingTop={2}>
              <ListItemInput
                label={
                  intermediateSettings?.isAgreed &&
                  intermediateSettings.messageLog
                    ? getResource("emailCleaner.descriptionAgreedByAt")
                        .replace("{USER}", intermediateSettings.messageLog.user)
                        .replace(
                          "{AT}",
                          intermediateSettings.messageLog.timeStamp
                        )
                    : getResource(
                        "emailCleaner.descriptionAgreeTermsAndConditions"
                      )
                }
              >
                <FormButton
                  label={
                    intermediateSettings?.isAgreed
                      ? getResource("emailCleaner.labelRead")
                      : getResource("emailCleaner.labelAgree")
                  }
                  disabled={!intermediateSettings?.emailCleanerEnabled}
                  onAction={setOpen.bind(null, true)}
                />
              </ListItemInput>
            </Grid2>
            <Grid2
              size={12}
              display="flex"
              justifyContent="flex-start"
              alignItems="center"
              gap={2}
              pt={4}
              paddingLeft={2}
            >
              {settings?.messageLog && (
                <Typography>
                  {getResource("emailCleaner.labelLastModificationByAt")
                    .replace("{USER}", intermediateSettings.messageLog.user)
                    .replace("{AT}", intermediateSettings.messageLog.timeStamp)}
                </Typography>
              )}
            </Grid2>
          </Grid2>
          <EmailCleanerOverlay open={folderConfigurationOpen}>
            <InboxFolderConfiguration
              accountId={accountId}
              isLoading={isLoading}
              folders={intermediateSettings.folderConfiguration}
              onClose={setFolderConfigurationOpen.bind(null, false)}
              handleFoldersChanged={handleSettingsChanged}
              handleGetUpdatedFolders={handleGetUpdatedFolders}
            />
          </EmailCleanerOverlay>
          <AgreementDialog
            open={open}
            agreementText={agreementText}
            agreed={intermediateSettings?.isAgreed ?? false}
            onAgree={onAggree.bind(null, true)}
            onClose={setOpen.bind(null, false)}
          />
        </Grid2>
      </Grid2>
    </DetailsView>
  );
};

export default EmailCleanerSettingsForm;
