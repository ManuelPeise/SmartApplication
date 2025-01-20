import { Grid2, IconButton, Tooltip, Typography } from "@mui/material";
import React from "react";
import DetailsView, { ButtonProps } from "src/_components/Layouts/DetailsView";
import {
  AccountDetailsState,
  ConnectionTestModel,
  EmailAccountModel,
} from "../EmailCleanerTypes";
import { EmailProviderTypeEnum } from "src/_lib/_enums/EmailProviderTypeEnum";
import LoadingIndicator from "src/_components/Loading/LoadingIndicator";
import ProviderSelection from "./ProviderSelection";
import { useI18n } from "src/_hooks/useI18n";
import TextInput from "src/_components/Input/TextInput";
import {
  CheckRounded,
  CloseRounded,
  EditRounded,
  VisibilityRounded,
} from "@mui/icons-material";
import { isEqual } from "lodash";
import { emailValidation } from "src/_lib/validation";

interface IProps {
  account: EmailAccountModel;
  isLoading: boolean;
  handleTestConnection: (model: ConnectionTestModel) => Promise<boolean>;
  handleSaveConnection: (model: EmailAccountModel) => Promise<boolean>;
  handleUpdateAccount?: (model: EmailAccountModel) => Promise<void>;
}

const AccountDetails: React.FC<IProps> = (props) => {
  const {
    account,
    isLoading,
    handleTestConnection,
    handleSaveConnection,
    handleUpdateAccount,
  } = props;
  const { getResource } = useI18n();

  const [formState, setFormState] = React.useState<AccountDetailsState>({
    mode: "view",
    account: account,
  });

  React.useEffect(() => {
    if (account) {
      setFormState({ mode: "view", account: account });
    }
  }, [account]);

  const handleFormStateChanged = React.useCallback(
    (partialState: Partial<AccountDetailsState>) => {
      setFormState({ ...formState, ...partialState });
    },
    [formState]
  );

  const handleAccountChanged = React.useCallback(
    (partialAccount: Partial<EmailAccountModel>) => {
      handleFormStateChanged({
        ...formState,
        account: {
          ...formState.account,
          ...partialAccount,
          accountName: "",
          emailAddress: "",
          password: "",
          connectionTestPassed: false,
        },
      });
    },
    [formState, handleFormStateChanged]
  );

  const onTestConnection = React.useCallback(async () => {
    await handleTestConnection({
      server: formState.account.server,
      port: formState.account.port,
      emailAddress: formState.account.emailAddress,
      password: formState.account.password,
    }).then((res) => {
      if (res === true) {
        setFormState({
          ...formState,
          account: { ...account, connectionTestPassed: res },
        });
      }
    });
  }, [handleTestConnection, formState, account]);

  const onSaveConnection = React.useCallback(async () => {
    if (formState.mode === "edit") {
      await handleUpdateAccount(formState.account);

      return;
    }
    await handleSaveConnection(formState?.account);
  }, [handleSaveConnection, handleUpdateAccount, formState]);

  const isModified = React.useMemo(() => {
    if (!account) {
      return false;
    }

    return !isEqual(account, formState?.account);
  }, [formState, account]);

  const isValid = React.useMemo((): boolean => {
    const isValidAuthData =
      formState?.account?.providerType !== EmailProviderTypeEnum.None &&
      emailValidation(formState?.account?.emailAddress) &&
      formState?.account?.password !== "" &&
      formState?.account?.accountName !== "" &&
      formState?.account?.connectionTestPassed;

    return isValidAuthData;
  }, [formState?.account]);

  const canTestConnection = React.useMemo((): boolean => {
    const isValidAuthData =
      formState?.account?.providerType !== EmailProviderTypeEnum.None &&
      emailValidation(formState?.account?.emailAddress) &&
      formState?.account?.password !== "" &&
      formState?.account?.accountName !== "" &&
      !formState?.account?.connectionTestPassed;

    return isValidAuthData;
  }, [formState?.account]);

  const handleCancel = React.useCallback(() => {
    setFormState({ mode: "view", account: account });
  }, [account]);

  const saveCancelButtonProps = React.useMemo((): ButtonProps[] => {
    return [
      {
        label: getResource("common.labelCancel"),
        disabled: formState.mode === "view" || !isModified,
        onAction: handleCancel,
      },
      {
        label: getResource("common.labelSave"),
        disabled: formState.mode === "view" || !isModified || !isValid,
        onAction: onSaveConnection,
      },
    ];
  }, [
    formState,
    isValid,
    isModified,
    handleCancel,
    getResource,
    onSaveConnection,
  ]);

  const additionalButtonProps = React.useMemo((): ButtonProps[] => {
    return [
      {
        label: getResource("emailCleaner.labelTestConnection"),
        disabled: !canTestConnection,
        onAction: onTestConnection,
      },
    ];
  }, [canTestConnection, getResource, onTestConnection]);

  if (formState?.account == null) {
    return null;
  }

  return (
    <DetailsView
      saveCancelButtonProps={saveCancelButtonProps}
      additionalButtonProps={additionalButtonProps}
    >
      <Grid2 size={12} p={4}>
        <LoadingIndicator isLoading={isLoading} />
        <Grid2 container size={12} p={1}>
          <Grid2
            p={2}
            size={12}
            display="flex"
            justifyContent="flex-end"
            alignItems="center"
          >
            {formState.mode === "view" ? (
              <IconButton
                onClick={handleFormStateChanged.bind(null, { mode: "edit" })}
              >
                <Tooltip
                  title={getResource("emailCleaner.labelEditMode")}
                  children={<VisibilityRounded />}
                />
              </IconButton>
            ) : (
              <IconButton
                onClick={handleFormStateChanged.bind(null, { mode: "view" })}
              >
                <Tooltip
                  title={getResource("emailCleaner.labelViewMode")}
                  children={<EditRounded />}
                />
              </IconButton>
            )}
          </Grid2>
          <ProviderSelection
            state={formState.account}
            disabled={formState.mode === "view"}
            handleChange={handleAccountChanged}
          />
          <Grid2 container size={12} spacing={1}>
            <Grid2 width="49%"></Grid2>
            <Grid2 width="49%">
              <TextInput
                label={getResource("emailCleaner.labelAccountName")}
                fullwidth
                disabled={
                  formState.account.providerType ===
                    EmailProviderTypeEnum.None || formState.mode === "view"
                }
                value={formState.account.accountName}
                onChange={(value) =>
                  handleFormStateChanged({
                    account: { ...formState.account, accountName: value },
                  })
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
                  formState.account.providerType ===
                    EmailProviderTypeEnum.None || formState.mode === "view"
                }
                value={formState.account.emailAddress}
                onChange={(value) =>
                  handleFormStateChanged({
                    account: {
                      ...formState.account,
                      emailAddress: value,
                      connectionTestPassed: false,
                    },
                  })
                }
              />
            </Grid2>
            <Grid2 width="49%">
              <TextInput
                label={getResource("settings.labelPassword")}
                fullwidth
                isPassword
                disabled={
                  formState.account.providerType ===
                    EmailProviderTypeEnum.None || formState.mode === "view"
                }
                value={formState.account.password}
                onChange={(value) =>
                  handleFormStateChanged({
                    account: {
                      ...formState.account,
                      password: value,
                      connectionTestPassed: false,
                    },
                  })
                }
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
              pt={4}
            >
              <Grid2 width="50%">
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
                {formState.account.connectionTestPassed ? (
                  <CheckRounded />
                ) : (
                  <CloseRounded />
                )}
              </Grid2>
            </Grid2>
          </Grid2>
          <Grid2
            size={12}
            display="flex"
            justifyContent="flex-start"
            alignItems="center"
            gap={2}
            pt={4}
          >
            {formState.account.messageLog && (
              <Typography>
                {getResource("emailCleaner.labelLastModificationByAt")
                  .replace("{USER}", formState.account.messageLog.user)
                  .replace("{AT}", formState.account.messageLog.timeStamp)}
              </Typography>
            )}
          </Grid2>
        </Grid2>
      </Grid2>
    </DetailsView>
  );
};

export default AccountDetails;
