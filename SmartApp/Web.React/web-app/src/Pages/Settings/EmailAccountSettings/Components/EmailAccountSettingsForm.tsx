import { Box, List } from "@mui/material";
import React from "react";
import { EmailAccountSettingsModel } from "../../types";
import ListItemInput from "src/_components/Lists/ListItemInput";
import { useI18n } from "src/_hooks/useI18n";
import TextInput from "src/_components/Input/TextInput";
import { EmailProviderTypeEnum } from "src/_lib/_enums/EmailProviderTypeEnum";
import FormButton from "src/_components/Buttons/FormButton";
import Dropdown, { DropDownItem } from "src/_components/Input/Dropdown";
import { emailProviderSettings } from "src/_lib/Settings/EmailProviderSettings";
import EmailAccountHeaderListItem from "./EmailAccountHeaderListItem";
import { emailValidation } from "src/_lib/validation";
import EmailAccountModificationLog from "./EmailAccountModificationLog";
import { isEqual } from "lodash";

interface IProps {
  index: number;
  selectedIndex: number;
  model?: EmailAccountSettingsModel;
  mode: "view" | "edit";
  onSave: (settings: EmailAccountSettingsModel) => Promise<void>;
}

const EmailAccountSettingsForm: React.FC<IProps> = (props) => {
  const { index, selectedIndex, model, mode, onSave } = props;
  const { getResource } = useI18n();

  const [formMode, setFormMode] = React.useState(mode);
  const [formState, setFormState] =
    React.useState<EmailAccountSettingsModel>(model);

  React.useEffect(() => {
    if (model) {
      setFormState(model);
    }
  }, [model]);

  const isModified = React.useMemo(() => {
    return isEqual(model, formState);
  }, [model, formState]);

  const handleChange = React.useCallback(
    (partialModel: Partial<EmailAccountSettingsModel>) => {
      setFormState({ ...formState, ...partialModel });
    },
    [formState]
  );

  const handleReset = React.useCallback(() => {
    setFormState(model);
  }, [model]);
  const toggleMode = React.useCallback((mode: "view" | "edit") => {
    setFormMode(mode);
  }, []);

  const isValid = React.useMemo((): boolean => {
    const isValidAuthData =
      formState.providerType !== EmailProviderTypeEnum.None &&
      emailValidation(formState.emailAddress) &&
      formState.password !== "" &&
      formState.accountName !== "";

    return isValidAuthData;
  }, [
    formState.accountName,
    formState.emailAddress,
    formState.password,
    formState.providerType,
  ]);

  const isReadonly = formMode !== "edit";

  const providerDropdownItems = React.useMemo((): DropDownItem[] => {
    const items: DropDownItem[] = emailProviderSettings.map((s, index) => {
      return {
        key: index,
        label: getResource(s.resourceKey),
      };
    });

    return items;
  }, [getResource]);

  const handleProviderChanged = React.useCallback(
    (type: EmailProviderTypeEnum) => {
      const providerSettings = emailProviderSettings.find(
        (x) => x.type === type
      );

      handleChange({
        server: providerSettings.server,
        port: providerSettings.port,
        providerType: providerSettings.type,
        emailAddress:
          type === EmailProviderTypeEnum.None ? "" : formState.emailAddress,
        password: type === EmailProviderTypeEnum.None ? "" : formState.password,
      });
    },
    [formState, handleChange]
  );

  const handleSaveSettings = React.useCallback(async () => {
    await onSave(formState).then((res) => {});
  }, [formState, onSave]);

  if (index !== selectedIndex) {
    return null;
  }

  return (
    <Box
      id="email-account-settings-form"
      sx={{
        display: "flex",
        flexDirection: "column",
        justifyContent: "space-between",
        height: "100%",
        minHeight: "750px",
        padding: 4,
      }}
    >
      <List>
        <EmailAccountHeaderListItem
          key="email-account-header"
          toggleFormModeDisabled={formState.id === -1}
          toggleLabel={getResource("settings.labelToggleMode")}
          formMode={formMode}
          providerType={formState.providerType}
          toggleMode={toggleMode}
        />

        <ListItemInput
          label={getResource("settings.descriptionSelectProvider")}
        >
          <Box width="100%" paddingTop={2}>
            <Dropdown
              fullWidth
              disabled={isReadonly}
              items={providerDropdownItems}
              value={formState.providerType}
              onChange={(type) => handleProviderChanged(type)}
            />
          </Box>
        </ListItemInput>
        <ListItemInput label={getResource("settings.descriptionAccountName")}>
          <TextInput
            label={getResource("settings.labelAccountName")}
            fullwidth
            disabled={
              isReadonly ||
              formState.providerType === EmailProviderTypeEnum.None
            }
            value={formState.accountName}
            onChange={(value) => handleChange({ accountName: value })}
          />
        </ListItemInput>
        <ListItemInput label={getResource("settings.descriptionEmail")}>
          <TextInput
            label={getResource("settings.labelEmail")}
            fullwidth
            disabled={
              isReadonly ||
              formState.providerType === EmailProviderTypeEnum.None
            }
            value={formState.emailAddress}
            onChange={(value) => handleChange({ emailAddress: value })}
          />
        </ListItemInput>
        <ListItemInput label={getResource("settings.descriptionPassword")}>
          <TextInput
            label={getResource("settings.labelPassword")}
            fullwidth
            isPassword
            disabled={
              isReadonly ||
              formState.providerType === EmailProviderTypeEnum.None
            }
            value={formState.password}
            onChange={(value) => handleChange({ password: value })}
          />
        </ListItemInput>
        {formState.messageLog && (
          <EmailAccountModificationLog
            caption={getResource("settings.captionModificationLog")}
            logMessage={getResource("settings.labelLastAccountModificationByAt")
              .replace("{User}", formState.messageLog.user)
              .replace("{TimeStamp}", formState.messageLog.timeStamp)}
          />
        )}
      </List>
      <Box height="25%">
        <Box
          width="100%"
          display="flex"
          flexDirection="row"
          justifyContent="flex-end"
          paddingTop={4}
          gap={2}
        >
          <FormButton
            label={getResource("common.labelCancel")}
            disabled={isModified}
            onAction={handleReset}
          />
          <FormButton
            label={getResource("common.labelSave")}
            disabled={isModified || !isValid}
            onAction={handleSaveSettings}
          />
        </Box>
      </Box>
    </Box>
  );
};

export default EmailAccountSettingsForm;
