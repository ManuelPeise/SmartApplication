import { Grid2, Paper, Typography } from "@mui/material";
import React from "react";
import { ConnectionTestModel, EmailAccountModel } from "../EmailCleanerTypes";
import { EmailProviderTypeEnum } from "src/_lib/_enums/EmailProviderTypeEnum";
import ProviderSelection from "./ProviderSelection";
import TextInput from "src/_components/Input/TextInput";
import { useI18n } from "src/_hooks/useI18n";
import { CheckRounded, CloseRounded } from "@mui/icons-material";
import FormButton from "src/_components/Buttons/FormButton";
import { isEqual } from "lodash";
import { emailValidation } from "src/_lib/validation";
import LoadingIndicator from "src/_components/Loading/LoadingIndicator";

interface IProps {
  userId: number;
  account?: EmailAccountModel;
  isLoading: boolean;
  handleTestConnection: (model: ConnectionTestModel) => Promise<boolean>;
  handleSaveConnection: (model: EmailAccountModel) => Promise<boolean>;
  handleClose: () => void;
}

const AddEmailAccountForm: React.FC<IProps> = (props) => {
  const {
    userId,
    account,
    isLoading,
    handleSaveConnection,
    handleTestConnection,
    handleClose,
  } = props;
  const { getResource } = useI18n();

  const fallbackModel = React.useMemo((): EmailAccountModel => {
    return {
      id: -1,
      userId: userId,
      accountName: "",
      providerType: EmailProviderTypeEnum.None,
      server: "",
      port: 993,
      emailAddress: "",
      password: "",
      connectionTestPassed: false,
      messageLog: null,
      settings: null,
      emailAddressMappings: [],
    };
  }, [userId]);

  const [interMediateAccountState, setInterMediateAccountState] =
    React.useState<EmailAccountModel>(account ?? fallbackModel);

  React.useEffect(() => {
    if (account) {
      setInterMediateAccountState(account);
    }
  }, [account]);

  const handleAccountChanged = React.useCallback(
    (partialAccount: Partial<EmailAccountModel>) => {
      setInterMediateAccountState({
        ...interMediateAccountState,
        ...partialAccount,
      });
    },
    [interMediateAccountState]
  );

  const handleCancelClick = React.useCallback(() => {
    handleClose();
    setInterMediateAccountState(!account ? fallbackModel : account);
  }, [account, fallbackModel, handleClose]);

  const onTestConnection = React.useCallback(async () => {
    await handleTestConnection({
      server: interMediateAccountState.server,
      port: interMediateAccountState.port,
      emailAddress: interMediateAccountState.emailAddress,
      password: interMediateAccountState.password,
    }).then((res) => {
      if (res === true) {
        setInterMediateAccountState({
          ...interMediateAccountState,
          connectionTestPassed: res,
        });
      }
    });
  }, [handleTestConnection, interMediateAccountState]);

  const onSaveConnection = React.useCallback(async () => {
    await handleSaveConnection(interMediateAccountState).then((res) => {
      if (res) {
        handleCancelClick();
      }
    });
  }, [handleSaveConnection, handleCancelClick, interMediateAccountState]);

  const isModified = React.useMemo(() => {
    return !isEqual(
      !account ? fallbackModel : account,
      interMediateAccountState
    );
  }, [account, fallbackModel, interMediateAccountState]);

  const isValid = React.useMemo((): boolean => {
    const isValidAuthData =
      interMediateAccountState.providerType !== EmailProviderTypeEnum.None &&
      emailValidation(interMediateAccountState.emailAddress) &&
      interMediateAccountState.password !== "" &&
      interMediateAccountState.accountName !== "" &&
      interMediateAccountState.connectionTestPassed;

    return isValidAuthData;
  }, [interMediateAccountState]);

  const canTestConnection = React.useMemo((): boolean => {
    const isValidAuthData =
      interMediateAccountState.providerType !== EmailProviderTypeEnum.None &&
      emailValidation(interMediateAccountState.emailAddress) &&
      interMediateAccountState.password !== "" &&
      interMediateAccountState.accountName !== "" &&
      !interMediateAccountState.connectionTestPassed;

    return isValidAuthData;
  }, [interMediateAccountState]);

  return (
    <Grid2
      id="email-account-form"
      p={2}
      size={{ xs: 12, sm: 12, md: 12, lg: 6, xl: 6 }}
    >
      <Paper sx={{ width: "100%", padding: 2 }}>
        <LoadingIndicator isLoading={isLoading} />
        <Grid2 container size={12} p={1}>
          <ProviderSelection
            state={interMediateAccountState}
            disabled={false}
            handleChange={handleAccountChanged}
          />
          <Grid2 container size={12} spacing={1}>
            <Grid2 width="49%"></Grid2>
            <Grid2 width="49%">
              <TextInput
                label={getResource("emailCleaner.labelAccountName")}
                fullwidth
                disabled={
                  interMediateAccountState.providerType ===
                  EmailProviderTypeEnum.None
                }
                value={interMediateAccountState.accountName}
                onChange={(value) =>
                  handleAccountChanged({ accountName: value })
                }
              />
            </Grid2>
          </Grid2>
          <Grid2 container size={12} spacing={1}>
            <Grid2 width="49%">
              <TextInput
                label={getResource("settings.labelEmail")}
                fullwidth
                disabled={
                  interMediateAccountState.providerType ===
                  EmailProviderTypeEnum.None
                }
                value={interMediateAccountState.emailAddress}
                onChange={(value) =>
                  handleAccountChanged({ emailAddress: value })
                }
              />
            </Grid2>
            <Grid2 width="49%">
              <TextInput
                label={getResource("settings.labelPassword")}
                fullwidth
                isPassword
                disabled={
                  interMediateAccountState.providerType ===
                  EmailProviderTypeEnum.None
                }
                value={interMediateAccountState.password}
                onChange={(value) => handleAccountChanged({ password: value })}
              />
            </Grid2>
          </Grid2>
          <Grid2 container size={12} spacing={1}>
            <Grid2
              size={12}
              display="flex"
              justifyContent="flex-end"
              alignItems="center"
              gap={2}
              pt={2}
            >
              <Grid2 width="49%">
                <Typography>
                  {getResource("emailCleaner.labelConnectionTestpassed")}
                </Typography>
              </Grid2>
              <Grid2
                width="49%"
                display="flex"
                justifyContent="flex-end"
                paddingRight={2}
              >
                {interMediateAccountState.connectionTestPassed ? (
                  <CheckRounded />
                ) : (
                  <CloseRounded />
                )}
              </Grid2>
            </Grid2>
          </Grid2>
          <Grid2 container size={12} spacing={1} pt={4}>
            <Grid2 width="49%">
              <FormButton
                label={getResource("emailCleaner.labelTestConnection")}
                disabled={
                  !canTestConnection ||
                  interMediateAccountState.connectionTestPassed
                }
                onAction={onTestConnection}
              />
            </Grid2>
            <Grid2 width="49%" display="flex" justifyContent="flex-end" gap={2}>
              <FormButton
                label={getResource("common.labelCancel")}
                disabled={false}
                onAction={handleCancelClick}
              />
              <FormButton
                label={getResource("common.labelSave")}
                disabled={
                  !isValid ||
                  !isModified ||
                  !interMediateAccountState.connectionTestPassed
                }
                onAction={onSaveConnection}
              />
            </Grid2>
          </Grid2>
        </Grid2>
      </Paper>
    </Grid2>
  );
};

export default AddEmailAccountForm;
