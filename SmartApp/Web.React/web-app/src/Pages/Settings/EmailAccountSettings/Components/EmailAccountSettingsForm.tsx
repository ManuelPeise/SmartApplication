import { Box, List } from "@mui/material";
import React from "react";
import { useFormState } from "src/_hooks/useFormState";
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

interface IProps {
  index: number;
  selectedIndex: number;
  model?: EmailAccountSettingsModel;
  initModel: EmailAccountSettingsModel;
  mode: "view" | "edit";
  onSave: (
    settings: EmailAccountSettingsModel
  ) => Promise<EmailAccountSettingsModel>;
  updateAccountSettings: (
    account: EmailAccountSettingsModel,
    initModel: EmailAccountSettingsModel,
    index: number
  ) => void;
}

const EmailAccountSettingsForm: React.FC<IProps> = (props) => {
  const {
    index,
    selectedIndex,
    model,
    initModel,
    mode,
    onSave,
    updateAccountSettings,
  } = props;
  const { getResource } = useI18n();

  const [formMode, setFormMode] = React.useState(mode);

  const toggleMode = React.useCallback((mode: "view" | "edit") => {
    setFormMode(mode);
  }, []);

  const validationCallback = React.useCallback(
    (model: EmailAccountSettingsModel): boolean => {
      const isValidAuthData =
        model.providerType !== EmailProviderTypeEnum.None
          ? emailValidation(model.emailAddress) &&
            model.password !== "" &&
            model.accountName !== ""
          : model.emailAddress === "" &&
            model.password === "" &&
            model.accountName !== "";

      return isValidAuthData;
    },
    []
  );

  const form = useFormState<EmailAccountSettingsModel>(
    model,
    validationCallback
  );

  const isReadonly = formMode !== "edit";

  const { formState, isDirty, isValid } = React.useMemo(() => {
    return form.subScribe();
  }, [form]);

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

      form.handleUpdatePartial({
        server: providerSettings.server,
        port: providerSettings.port,
        providerType: providerSettings.type,
        emailAddress:
          type === EmailProviderTypeEnum.None ? "" : formState.emailAddress,
        password: type === EmailProviderTypeEnum.None ? "" : formState.password,
      });
    },
    [form, formState]
  );

  const handleSaveSettings = React.useCallback(async () => {
    await onSave(formState).then((res) => {
      if (res !== null) {
        form.handleUpdate(model);
        updateAccountSettings(res, initModel, index);
      }
    });
  }, [form, model, formState, initModel, index, onSave, updateAccountSettings]);

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
            onChange={(value) =>
              form.handleUpdatePartial({ accountName: value })
            }
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
            onChange={(value) =>
              form.handleUpdatePartial({ emailAddress: value })
            }
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
            onChange={(value) => form.handleUpdatePartial({ password: value })}
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
            disabled={!isDirty}
            onAction={form.handleReset}
          />
          <FormButton
            label={getResource("common.labelSave")}
            disabled={!isDirty || !isValid}
            onAction={handleSaveSettings}
          />
        </Box>
      </Box>
    </Box>
  );
};

export default EmailAccountSettingsForm;
