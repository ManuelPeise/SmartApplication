import { Box, Switch } from "@mui/material";
import React from "react";
import { useFormState } from "src/_hooks/useFormState";
import EmailProviderSelection from "./EmailProviderSelection";
import Dropdown, { DropDownItem } from "src/_components/_input/Dropdown";
import { emailProviders } from "../emailProviders";
import ListItemInput from "src/_components/_list/ListItemInput";
import TextInput from "src/_components/_input/TextInput";
import FormButton from "src/_components/_buttons/FormButton";
import { useEmailConfigurationContextProvider } from "src/_hooks/userContextProvider";
import moment from "moment";
import { colors } from "src/_lib/colors";
import { emailValidation } from "src/_lib/validation";
import { EmailProviderConfiguration } from "../types";
import { EmailProviderTypeEnum } from "../_enums/EmailProviderTypeEnum";
import { EmailProviderConfigurationStateEnum } from "../_enums/EmailProviderConfigurationStateEnum";

interface IProps {
  tabIndex: number;
  selectedTab: number;
  isLoading: boolean;
  configuration: EmailProviderConfiguration;
  getResource: (key: string) => string;
}

const EmailProviderAccountConfigurationForm: React.FC<IProps> = (props) => {
  const { tabIndex, selectedTab, isLoading, configuration, getResource } =
    props;

  const validationCallback = React.useCallback(
    (config: EmailProviderConfiguration) => {
      return (
        config.provider.providerType !== EmailProviderTypeEnum.None &&
        config.name !== "" &&
        emailValidation(config.emailAddress) &&
        config.password !== ""
      );
    },
    []
  );

  const { handleProviderConnectionTest, handleEstablishConnection } =
    useEmailConfigurationContextProvider();
  const form = useFormState<EmailProviderConfiguration>(
    configuration,
    validationCallback
  );

  const { formState, isValid, isDirty } = React.useMemo(() => {
    return form.subScribe();
  }, [form]);

  const onTestConnection = React.useCallback(async () => {
    const canConnect = await handleProviderConnectionTest(formState);
    console.log("Can connect", canConnect);
    form.handleUpdatePartial({ connectionTestPassed: canConnect });
  }, [form, formState, handleProviderConnectionTest]);

  const onEstablishConnection = React.useCallback(async () => {
    await handleEstablishConnection(formState);
  }, [formState, handleEstablishConnection]);

  const handleDisconnect = React.useCallback(() => {
    form.handleUpdatePartial({
      status: EmailProviderConfigurationStateEnum.Pending,
    });
  }, [form]);

  const providerDropdownItems = React.useMemo((): DropDownItem[] => {
    return emailProviders.map((p) => {
      return {
        key: p.providerType,
        label: p.displayName,
        disabled: p.providerType === formState.provider.providerType,
      };
    });
  }, [formState.provider.providerType]);

  const connectionEstablished = React.useMemo(() => {
    return formState.status === EmailProviderConfigurationStateEnum.Established;
  }, [formState]);

  const connectionPending = React.useMemo(() => {
    return formState.status === EmailProviderConfigurationStateEnum.Pending;
  }, [formState]);

  if (tabIndex !== selectedTab) {
    return null;
  }

  return (
    <Box
      padding={0}
      height="100%"
      display="flex"
      flexDirection="column"
      justifyContent="space-between"
    >
      <Box display="flex" flexDirection="column">
        <EmailProviderSelection
          imageSrc={formState.provider.logo}
          childWidth="12rem"
        >
          <Dropdown
            fullWidth
            disabled={isLoading || connectionEstablished}
            items={providerDropdownItems}
            value={formState.provider.providerType}
            onChange={(type) =>
              form.handleUpdatePartial({
                provider: emailProviders.find((p) => p.providerType === type),
              })
            }
          />
        </EmailProviderSelection>
        <ListItemInput
          description={getResource(
            "administration.descriptionEmailProvideConnectionName"
          )}
          childWidth="12rem"
        >
          <TextInput
            value={formState.name}
            fullwidth
            label={getResource(
              "administration.labelEmailProvideConnectionName"
            )}
            disabled={
              formState.provider.providerType === EmailProviderTypeEnum.None ||
              connectionEstablished ||
              isLoading
            }
            onChange={(value) => form.handleUpdatePartial({ name: value })}
          />
        </ListItemInput>
        <ListItemInput
          description={getResource("administration.descriptionEmail")}
          childWidth="12rem"
        >
          <TextInput
            value={formState.emailAddress}
            fullwidth
            label={getResource("administration.labelEmail")}
            disabled={
              formState.provider.providerType === EmailProviderTypeEnum.None ||
              connectionEstablished ||
              isLoading
            }
            onChange={(value) =>
              form.handleUpdatePartial({ emailAddress: value })
            }
          />
        </ListItemInput>
        <ListItemInput
          description={getResource("administration.descriptionPassword")}
          childWidth="12rem"
        >
          <TextInput
            value={formState.password}
            fullwidth
            isPassword
            label={getResource("administration.labelPassword")}
            disabled={
              formState.provider.providerType === EmailProviderTypeEnum.None ||
              connectionEstablished ||
              isLoading
            }
            onChange={(value) => form.handleUpdatePartial({ password: value })}
          />
        </ListItemInput>
        <ListItemInput
          description={getResource("administration.descriptionTestConnection")}
          childWidth="12rem"
        >
          <FormButton
            label={getResource("administration.labelTestConnection")}
            disabled={
              connectionEstablished ||
              !isValid ||
              formState.provider.providerType === EmailProviderTypeEnum.None
            }
            onAction={onTestConnection}
          />
        </ListItemInput>
        <ListItemInput
          description={getResource(
            "administration.descriptionCollectAiTrainingData"
          )}
          childWidth="12rem"
        >
          <Switch
            checked={formState.allowCollectAiTrainingData ?? false}
            disabled={
              formState.provider.providerType === EmailProviderTypeEnum.None
            }
            onChange={(value) =>
              form.handleUpdatePartial({
                allowCollectAiTrainingData: value.target.checked,
              })
            }
            color="success"
          />
        </ListItemInput>
        {formState?.connectionInfo && (
          <ListItemInput
            cssTextProperties={{
              fontSize: "16px",
              fontStyle: "italic",
              color: colors.typography.darkgray,
            }}
            description={getResource(
              "administration.labelProviderConnectionEstablishedAtBy"
            )
              .replace("{user}", formState.connectionInfo.updatedBy)
              .replace(
                "{at}",
                moment(formState.connectionInfo?.updatedAt).format(
                  "DD.MM.YYYY HH:MM:SS"
                )
              )}
            childWidth="12rem"
          ></ListItemInput>
        )}
      </Box>
      <Box
        padding={3}
        sx={{ display: "flex", justifyContent: "space-between" }}
      >
        <Box display="flex" alignItems="baseline" gap={2}>
          <FormButton
            key="disconnect-button"
            label={getResource("administration.labelDisconnect")}
            disabled={connectionPending}
            onAction={handleDisconnect}
          />
        </Box>
        <Box display="flex" alignItems="baseline" gap={2}>
          <FormButton
            key="cancel-button"
            label={getResource("common.labelCancel")}
            disabled={!isDirty}
            onAction={form.handleReset}
          />
          <FormButton
            key="save-button"
            label={getResource("common.labelSave")}
            disabled={!formState.connectionTestPassed || !isDirty}
            onAction={onEstablishConnection}
          />
        </Box>
      </Box>
    </Box>
  );
};

export default EmailProviderAccountConfigurationForm;
