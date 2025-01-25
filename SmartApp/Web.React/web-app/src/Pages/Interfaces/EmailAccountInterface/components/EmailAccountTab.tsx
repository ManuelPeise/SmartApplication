import React from "react";
import {
  EmailAccountConnectionTestRequest,
  EmailAccountSettings,
  EmailMappingUpdateStatus,
} from "../types";
import DetailsView, { ButtonProps } from "src/_components/Layouts/DetailsView";
import { Grid2 } from "@mui/material";
import ProviderSelection from "./ProviderSelection";
import ProviderAuthentication from "./ProviderAuthentication";
import EmailAccountName from "./EmailAccountName";
import ProviderConnectionInfo from "./ProviderConnectionInfo";
import { useI18n } from "src/_hooks/useI18n";
import { emailValidation } from "src/_lib/validation";
import { isEqual } from "lodash";
import EmailMappingImportAlert from "./EmailMappingImportAlert";
import AccountAiSettings from "./AccountAiSettings";

interface IProps {
  tabIndex: number;
  selectedTab: number;
  isLoading: boolean;
  minHeight: number;
  state: EmailAccountSettings;
  handleTestConnection: (
    request: EmailAccountConnectionTestRequest
  ) => Promise<boolean>;
  handleSaveConnection: (connection: EmailAccountSettings) => Promise<void>;
  handleUpdateMappingTable: (settingsGuid: string) => Promise<boolean>;
}

const EmailAccountTab: React.FC<IProps> = (props) => {
  const {
    tabIndex,
    selectedTab,
    minHeight,
    isLoading,
    state,
    handleTestConnection,
    handleSaveConnection,
    handleUpdateMappingTable,
  } = props;
  const { getResource } = useI18n();

  const [intermediateState, setIntermediateState] =
    React.useState<EmailAccountSettings>(state);

  const [importState, setImportState] =
    React.useState<EmailMappingUpdateStatus>({
      open: false,
      success: false,
    });

  const handleImportState = React.useCallback(
    (partialState: Partial<EmailMappingUpdateStatus>) => {
      setImportState({ ...importState, ...partialState });
    },
    [importState]
  );

  const handleChange = React.useCallback(
    (partialState: Partial<EmailAccountSettings>) => {
      setIntermediateState({ ...intermediateState, ...partialState });
    },
    [intermediateState]
  );

  const handleReset = React.useCallback(() => {
    setIntermediateState(state);
  }, [state]);

  const onTestConnection = React.useCallback(async () => {
    await handleTestConnection({
      server: intermediateState.server,
      port: intermediateState.port,
      emailAddress: intermediateState.emailAddress,
      password: intermediateState.password,
    }).then((res) => {
      handleChange({ connectionTestPassed: res });
    });
  }, [
    handleChange,
    handleTestConnection,
    intermediateState.emailAddress,
    intermediateState.password,
    intermediateState.port,
    intermediateState.server,
  ]);

  const onUpdateMappingTable = React.useCallback(async () => {
    await handleUpdateMappingTable(intermediateState.settingsGuid).then(
      (res) => {
        handleImportState({ open: true, success: res });
      }
    );
  }, [
    handleUpdateMappingTable,
    handleImportState,
    intermediateState.settingsGuid,
  ]);

  const isModified = React.useMemo((): boolean => {
    return !isEqual(state, intermediateState);
  }, [state, intermediateState]);

  const connectionTestDisabled = React.useMemo((): boolean => {
    const isValidAccountName = intermediateState.accountName !== "";

    const isValidAccountData =
      intermediateState.password === undefined
        ? false
        : emailValidation(intermediateState.emailAddress) &&
          intermediateState?.password !== "";

    return (
      !isValidAccountName ||
      !isValidAccountData ||
      intermediateState.connectionTestPassed === true
    );
  }, [
    intermediateState.accountName,
    intermediateState.connectionTestPassed,
    intermediateState.emailAddress,
    intermediateState.password,
  ]);

  const saveCancelButtonProps = React.useMemo((): ButtonProps[] => {
    return [
      {
        label: getResource("common.labelCancel"),
        disabled: !isModified,
        onAction: handleReset,
      },
      {
        label: getResource("common.labelSave"),
        disabled: !isModified || !intermediateState.connectionTestPassed,
        onAction: handleSaveConnection.bind(null, intermediateState),
      },
    ];
  }, [
    isModified,
    intermediateState,
    handleSaveConnection,
    getResource,
    handleReset,
  ]);

  const updateMappingTableDisabled = React.useMemo(() => {
    return (
      !intermediateState.connectionTestPassed ||
      !intermediateState.settingsGuid.length ||
      !intermediateState.emailAccountAiSettings.useAiTargetFolderPrediction ||
      isLoading ||
      isModified
    );
  }, [
    intermediateState.connectionTestPassed,
    intermediateState.emailAccountAiSettings.useAiTargetFolderPrediction,
    intermediateState.settingsGuid.length,
    isLoading,
    isModified,
  ]);

  const additionalButtonProps = React.useMemo((): ButtonProps[] => {
    return [
      {
        label: getResource("interface.labelTestConnection"),
        disabled: connectionTestDisabled,
        onAction: onTestConnection,
      },
      {
        label: getResource("interface.labelUpdateMappingTable"),
        disabled: updateMappingTableDisabled,
        onAction: onUpdateMappingTable,
      },
    ];
  }, [
    getResource,
    connectionTestDisabled,
    onTestConnection,
    updateMappingTableDisabled,
    onUpdateMappingTable,
  ]);

  React.useEffect(() => {
    setIntermediateState(state);
  }, [state]);

  if (selectedTab !== tabIndex) {
    return null;
  }

  console.log("Render tab...");
  return (
    <DetailsView
      saveCancelButtonProps={saveCancelButtonProps}
      additionalButtonProps={additionalButtonProps}
    >
      <Grid2
        sx={{
          scrollbarWidth: "none",
          msOverflowStyle: "none",
          overflow: "hidden",
        }}
        container
        display="flex"
        flexDirection="column"
        minWidth="650px"
        minHeight={minHeight - 100}
        width="inherit"
        padding={2}
      >
        {importState.open && (
          <EmailMappingImportAlert
            key="email-mapping-import-alert"
            open={importState.open}
            success={importState.success}
            timeOut={3000}
            text={
              importState.success
                ? getResource("interface.labelMappingUpdateSuccessfull")
                : getResource("interface.labelMappingUpdateFailedfull")
            }
            handleClose={handleImportState.bind(null, { open: false })}
          />
        )}
        <ProviderSelection
          key="provider-selection"
          disabled={false}
          state={intermediateState}
          handleChange={handleChange}
        />
        <EmailAccountName
          key="email-account-name"
          state={intermediateState}
          handleChange={handleChange}
        />
        <ProviderAuthentication
          key="provider-authentication"
          state={intermediateState}
          handleChange={handleChange}
        />
        <ProviderConnectionInfo
          key="provider-connection-info"
          state={intermediateState}
        />
        <AccountAiSettings
          key="account-ai-settings"
          aiSettings={intermediateState.emailAccountAiSettings}
          handleChange={handleChange}
        />
      </Grid2>
    </DetailsView>
  );
};

export default EmailAccountTab;
