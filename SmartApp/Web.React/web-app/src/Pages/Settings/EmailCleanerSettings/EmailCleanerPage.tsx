import { Box, Paper } from "@mui/material";
import React from "react";
import { DropDownItem } from "src/_components/Input/Dropdown";
import EmailCleanerSettingsToolbar from "./Components/EmailCleanerSettingsToolbar";
import { EmailCleanerSettings } from "../Types/EmailCleanerConfiguration";
import ListItemInput from "src/_components/Lists/ListItemInput";
import { useI18n } from "src/_hooks/useI18n";
import SwitchInput from "src/_components/Input/SwitchInput";
import { colors } from "src/_lib/colors";
import NumberInput from "src/_components/Input/NumberInput";
import FormButton from "src/_components/Buttons/FormButton";
import { isEqual } from "lodash";
import { useNavigate } from "react-router-dom";
import { browserRoutes } from "src/_lib/Router/RouterUtils";

interface IProps {
  accountDropdownItems: DropDownItem[];
  selectedAccount: number;
  settings: EmailCleanerSettings;
  isLoading: boolean;
  handleAccountChanged: React.Dispatch<React.SetStateAction<number>>;
  handleSaveSettings: (settings: EmailCleanerSettings) => Promise<void>;
}

const listItemPaddingTop = 1;

const EmailCleanerPage: React.FC<IProps> = (props) => {
  const {
    accountDropdownItems,
    selectedAccount,
    settings,
    isLoading,
    handleAccountChanged,
    handleSaveSettings,
  } = props;

  const navigate = useNavigate();
  const { getResource } = useI18n();
  const [intermediateSettings, setIntermediateSettings] =
    React.useState<EmailCleanerSettings>(settings);

  React.useEffect(() => {
    if (settings) {
      setIntermediateSettings(settings);
    }
  }, [settings]);

  const handleSettingsChanged = React.useCallback(
    (partialSettings: Partial<EmailCleanerSettings>) => {
      setIntermediateSettings({ ...intermediateSettings, ...partialSettings });
    },
    [intermediateSettings]
  );

  const isModified = React.useMemo(() => {
    return isEqual(intermediateSettings, settings);
  }, [settings, intermediateSettings]);

  return (
    <Box width="100%" height="100%">
      <EmailCleanerSettingsToolbar
        accountDropdownItems={accountDropdownItems}
        selectedAccount={selectedAccount}
        accountDropdownDisabled={isLoading}
        handleAccountChanged={handleAccountChanged}
      />
      <Box maxHeight="700px" minHeight="500px" padding={2}>
        <Paper sx={{ height: "100%", width: "100%" }} elevation={2}>
          <Box
            minHeight={{
              xs: "auto",
              sm: "auto",
              md: "auto",
              lg: "680px",
              xl: "680px",
            }}
            height="100%"
            maxWidth="100%"
            padding={2}
            paddingLeft={4}
            paddingRight={4}
            display="flex"
            flexDirection="column"
            alignItems="baseline"
            justifyContent="flex-start"
          >
            <ListItemInput
              label={getResource(
                "settings.descriptionEmailCleanerSettingsEnabled"
              )}
            >
              <Box paddingTop={listItemPaddingTop}>
                <SwitchInput
                  checked={intermediateSettings?.enabled}
                  checkedColor={colors.button.enabled}
                  handleChange={(e) =>
                    handleSettingsChanged({ enabled: e.currentTarget.checked })
                  }
                />
              </Box>
            </ListItemInput>
            <ListItemInput
              label={getResource(
                "settings.descriptionEmailCleanerAllowLoadEmails"
              )}
            >
              <Box paddingTop={listItemPaddingTop}>
                <SwitchInput
                  checked={intermediateSettings?.allowReadEmails}
                  checkedColor={colors.button.enabled}
                  disabled={!intermediateSettings.enabled}
                  handleChange={(e) =>
                    handleSettingsChanged({
                      allowReadEmails: e.currentTarget.checked,
                      allowCreateEmailFolder: !e.currentTarget.checked
                        ? false
                        : intermediateSettings.allowCreateEmailFolder,
                      allowMoveEmails: !e.currentTarget.checked
                        ? false
                        : intermediateSettings.allowMoveEmails,
                      allowDeleteEmails: !e.currentTarget.checked
                        ? false
                        : intermediateSettings.allowDeleteEmails,
                      shareEmailDataToTrainAi: !e.currentTarget.checked
                        ? false
                        : intermediateSettings.shareEmailDataToTrainAi,
                      scheduleCleanup: !e.currentTarget.checked
                        ? false
                        : intermediateSettings.scheduleCleanup,
                    })
                  }
                />
              </Box>
            </ListItemInput>
            <ListItemInput
              label={getResource(
                "settings.descriptionEmailCleanerCreateEmailFolder"
              )}
            >
              <Box paddingTop={listItemPaddingTop}>
                <SwitchInput
                  checked={intermediateSettings?.allowCreateEmailFolder}
                  checkedColor={colors.button.enabled}
                  disabled={
                    !intermediateSettings.enabled ||
                    !intermediateSettings.allowReadEmails
                  }
                  handleChange={(e) =>
                    handleSettingsChanged({
                      allowCreateEmailFolder: e.currentTarget.checked,
                      allowMoveEmails: !e.currentTarget.checked
                        ? false
                        : intermediateSettings.allowMoveEmails,
                      allowDeleteEmails: !e.currentTarget.checked
                        ? false
                        : intermediateSettings.allowDeleteEmails,
                    })
                  }
                />
              </Box>
            </ListItemInput>
            <ListItemInput
              label={getResource("settings.descriptionEmailCleanerMoveEmails")}
            >
              <Box paddingTop={listItemPaddingTop}>
                <SwitchInput
                  checked={intermediateSettings?.allowMoveEmails}
                  checkedColor={colors.button.enabled}
                  disabled={
                    !intermediateSettings.enabled ||
                    !intermediateSettings?.allowCreateEmailFolder ||
                    !intermediateSettings.allowReadEmails
                  }
                  handleChange={(e) =>
                    handleSettingsChanged({
                      allowMoveEmails: e.currentTarget.checked,
                    })
                  }
                />
              </Box>
            </ListItemInput>
            <ListItemInput
              label={getResource(
                "settings.descriptionEmailCleanerDeleteEmails"
              )}
            >
              <Box paddingTop={listItemPaddingTop}>
                <SwitchInput
                  checked={intermediateSettings?.allowDeleteEmails}
                  checkedColor={colors.button.enabled}
                  disabled={
                    !intermediateSettings.enabled ||
                    !intermediateSettings?.allowCreateEmailFolder ||
                    !intermediateSettings.allowReadEmails
                  }
                  handleChange={(e) =>
                    handleSettingsChanged({
                      allowDeleteEmails: e.currentTarget.checked,
                    })
                  }
                />
              </Box>
            </ListItemInput>
            <ListItemInput
              label={getResource(
                "settings.descriptionEmailCleanerShareEmailDataToTrainAi"
              )}
            >
              <Box paddingTop={listItemPaddingTop}>
                <SwitchInput
                  checked={intermediateSettings?.shareEmailDataToTrainAi}
                  checkedColor={colors.button.enabled}
                  disabled={
                    !intermediateSettings.enabled ||
                    !intermediateSettings.allowReadEmails
                  }
                  handleChange={(e) =>
                    handleSettingsChanged({
                      shareEmailDataToTrainAi: e.currentTarget.checked,
                    })
                  }
                />
              </Box>
            </ListItemInput>
            <ListItemInput
              label={getResource(
                "settings.descriptionEmailCleanerScheduleCleanup"
              )}
            >
              <Box paddingTop={listItemPaddingTop}>
                <SwitchInput
                  checked={intermediateSettings?.scheduleCleanup}
                  checkedColor={colors.button.enabled}
                  disabled={
                    !intermediateSettings.enabled ||
                    !intermediateSettings.allowReadEmails
                  }
                  handleChange={(e) =>
                    handleSettingsChanged({
                      scheduleCleanup: e.currentTarget.checked,
                    })
                  }
                />
              </Box>
            </ListItemInput>
            <ListItemInput
              label={getResource(
                "settings.descriptionEmailCleanerScheduleCleanupAtHour"
              )}
            >
              <Box paddingTop={listItemPaddingTop}>
                <NumberInput
                  label={getResource("settings.labelHour")}
                  inputMode="numeric"
                  minValue={0}
                  maxValue={23}
                  value={intermediateSettings.scheduleCleanupAtHour.toString()}
                  disabled={
                    !intermediateSettings.enabled ||
                    !intermediateSettings.scheduleCleanup
                  }
                  onChange={(value) =>
                    handleSettingsChanged({
                      scheduleCleanupAtHour: parseInt(value),
                    })
                  }
                />
              </Box>
            </ListItemInput>
            <ListItemInput
              label={getResource(
                "settings.descriptionEmailCleanerEmailAddressMapping"
              )}
            >
              <Box paddingTop={2}>
                <FormButton
                  label={getResource("settings.labelShowEmailAddressMapping")}
                  disabled={!intermediateSettings.enabled}
                  onAction={() =>
                    navigate(
                      `${browserRoutes.emailCleanerMappings}${intermediateSettings.accountId}`
                    )
                  }
                />
              </Box>
            </ListItemInput>
          </Box>
          <Box
            padding={2}
            paddingRight={4}
            paddingLeft={4}
            display="flex"
            flexDirection="row"
            justifyContent="flex-end"
            gap={2}
          >
            <FormButton
              label={getResource("common.labelCancel")}
              disabled={isModified}
              onAction={setIntermediateSettings.bind(null, settings)}
            />
            <FormButton
              label={getResource("common.labelSave")}
              disabled={isModified}
              onAction={handleSaveSettings.bind(null, intermediateSettings)}
            />
          </Box>
        </Paper>
      </Box>
    </Box>
  );
};

export default EmailCleanerPage;
