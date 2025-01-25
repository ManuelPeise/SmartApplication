import React from "react";
import {
  EmailAccountConnectionTestRequest,
  EmailAccountSettings,
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

interface IProps {
  tabIndex: number;
  selectedTab: number;
  minHeight: number;
  state: EmailAccountSettings;
  handleTestConnection: (
    request: EmailAccountConnectionTestRequest
  ) => Promise<boolean>;
  handleSaveConnection: (connection: EmailAccountSettings) => Promise<void>;
}

const EmailAccountTab: React.FC<IProps> = (props) => {
  const {
    tabIndex,
    selectedTab,
    minHeight,
    state,
    handleTestConnection,
    handleSaveConnection,
  } = props;
  const { getResource } = useI18n();

  const [intermediateState, setIntermediateState] =
    React.useState<EmailAccountSettings>(state);

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

  const additionalButtonProps = React.useMemo((): ButtonProps[] => {
    return [
      {
        label: getResource("interface.labelTestConnection"),
        disabled: connectionTestDisabled,
        onAction: onTestConnection,
      },
    ];
  }, [connectionTestDisabled, getResource, onTestConnection]);

  React.useEffect(() => {
    setIntermediateState(state);
  }, [state]);

  if (selectedTab !== tabIndex) {
    return null;
  }

  return (
    <DetailsView
      saveCancelButtonProps={saveCancelButtonProps}
      additionalButtonProps={additionalButtonProps}
    >
      <Grid2
        container
        display="flex"
        flexDirection="column"
        minWidth="650px"
        minHeight={minHeight - 100}
        width="inherit"
        padding={2}
      >
        <ProviderSelection
          disabled={false}
          state={intermediateState}
          handleChange={handleChange}
        />
        <EmailAccountName
          state={intermediateState}
          handleChange={handleChange}
        />
        <ProviderAuthentication
          state={intermediateState}
          handleChange={handleChange}
        />
        <ProviderConnectionInfo state={intermediateState} />
      </Grid2>
    </DetailsView>
  );
};

export default EmailAccountTab;
