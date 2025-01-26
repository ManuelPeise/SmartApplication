import React from "react";
import DetailsView, { ButtonProps } from "src/_components/Layouts/DetailsView";
import {
  EmailCleanerInterfaceConfigurationUiModel,
  EmailCleanerUpdateModel,
} from "../types";
import {
  Box,
  Divider,
  Grid2,
  IconButton,
  List,
  Tooltip,
  Typography,
} from "@mui/material";
import { emailProviderSettings } from "src/_lib/Settings/EmailProviderSettings";
import { colors } from "src/_lib/colors";
import ListItemInput from "src/_components/Lists/ListItemInput";
import { useI18n } from "src/_hooks/useI18n";
import SwitchInput from "src/_components/Input/SwitchInput";
import { isEqual } from "lodash";
import { ChecklistRtlRounded, InfoOutlined } from "@mui/icons-material";
import { useNavigate } from "react-router-dom";
import { browserRoutes } from "src/_lib/Router/RouterUtils";
import NoDataPlaceholder from "src/_components/Placeholders/NoDataPlaceholder";

interface IProps {
  tabindex: number;
  selectedTab: number;
  minHeight: number;
  dataSet: EmailCleanerInterfaceConfigurationUiModel;
  handleUpdateConfiguration: (model: EmailCleanerUpdateModel) => Promise<void>;
}

const EmailCleanerInterfaceTab: React.FC<IProps> = (props) => {
  const {
    tabindex,
    selectedTab,
    dataSet,
    minHeight,
    handleUpdateConfiguration,
  } = props;
  const { getResource } = useI18n();
  const navigate = useNavigate();
  const [intermediateState, setIntermediateState] =
    React.useState<EmailCleanerInterfaceConfigurationUiModel>(dataSet);

  const providerSettings = React.useMemo(() => {
    return emailProviderSettings.find((x) => x.type === dataSet.providerType);
  }, [dataSet]);

  const handleSettingsChanged = React.useCallback(
    (partialState: Partial<EmailCleanerInterfaceConfigurationUiModel>) => {
      setIntermediateState({ ...intermediateState, ...partialState });
    },
    [intermediateState]
  );

  const onUpdate = React.useCallback(async () => {
    await handleUpdateConfiguration({
      settingsGuid: intermediateState.settingsGuid,
      emailCleanerEnabled: intermediateState.emailCleanerEnabled,
      useAiSpamPrediction: intermediateState.useAiSpamPrediction,
      useAiTargetFolderPrediction:
        intermediateState.useAiTargetFolderPrediction,
    });
  }, [intermediateState, handleUpdateConfiguration]);

  const onReset = React.useCallback(() => {
    handleSettingsChanged(dataSet);
  }, [dataSet, handleSettingsChanged]);

  React.useEffect(() => {
    setIntermediateState(dataSet);
  }, [dataSet]);

  const canSave = React.useMemo((): boolean => {
    return !isEqual(dataSet, intermediateState);
  }, [dataSet, intermediateState]);

  const saveCancelButtonProps = React.useMemo((): ButtonProps[] => {
    return [
      {
        label: getResource("common.labelCancel"),
        disabled: !canSave,
        onAction: onReset,
      },
      {
        label: getResource("common.labelSave"),
        disabled: !canSave,
        onAction: onUpdate,
      },
    ];
  }, [canSave, getResource, onReset, onUpdate]);

  if (selectedTab !== tabindex) {
    return null;
  }

  if (!intermediateState.connectionTestPassed) {
    return (
      <NoDataPlaceholder
        buttonLabel={getResource("interface.labelConfigure")}
        infoText={getResource("common.labelNoDataMessage").replace(
          "{MessageExtension}",
          getResource("interface.labelConnectionTestNotPassed")
        )}
        route={browserRoutes.emailAccountInterface}
      />
    );
  }

  return (
    <DetailsView saveCancelButtonProps={saveCancelButtonProps}>
      <Grid2
        sx={{
          scrollbarWidth: "none",
          msOverflowStyle: "none",
          overflow: "hidden",
          minWidth: "800px",
        }}
        container
        display="flex"
        flexDirection="column"
        minWidth="800px"
        minHeight={minHeight - 100}
        width="inherit"
        padding={2}
      >
        <Grid2 display="flex" flexDirection="column" width="100%">
          <Grid2
            display="flex"
            width="inherit"
            justifyContent="space-between"
            p={2}
          >
            <Box
              component="img"
              src={providerSettings.imageSrc}
              alt="provider logo"
              sx={{ width: "50px", height: "50px" }}
            />
            <Box>
              <Typography variant="body1">{dataSet.accountName}</Typography>
              <Typography
                variant="body2"
                sx={{ color: colors.typography.lightgray }}
              >
                {dataSet.emailAddress}
              </Typography>
            </Box>
          </Grid2>
          <Divider />
        </Grid2>
        <List disablePadding>
          <ListItemInput
            key="email-cleaner-enabled"
            marginTop="30px"
            label={getResource("interface.descriptionEmailCleanerEnabled")}
          >
            <SwitchInput
              disabled={!dataSet.connectionTestPassed}
              checked={intermediateState.emailCleanerEnabled}
              handleChange={(e) =>
                handleSettingsChanged({
                  emailCleanerEnabled: e.currentTarget.checked,
                  useAiSpamPrediction: !e.currentTarget.checked
                    ? false
                    : intermediateState.useAiSpamPrediction,
                  useAiTargetFolderPrediction: !e.currentTarget.checked
                    ? false
                    : intermediateState.useAiTargetFolderPrediction,
                })
              }
            />
          </ListItemInput>
          <ListItemInput
            key="use-ai-spam-prediction"
            marginTop="30px"
            label={getResource("interface.descriptionUseAiSpamPrediction")}
          >
            <SwitchInput
              disabled={true} // !intermediateState.emailCleanerEnabled
              checked={intermediateState.useAiSpamPrediction}
              handleChange={(e) =>
                handleSettingsChanged({
                  useAiSpamPrediction: e.currentTarget.checked,
                })
              }
            />
          </ListItemInput>
          <ListItemInput
            key="use-ai-target-folder-prediction"
            marginTop="30px"
            label={getResource(
              "interface.descriptionUseAiTargetFolderPrediction"
            )}
          >
            <SwitchInput
              disabled={true} // !intermediateState.emailCleanerEnabled
              checked={intermediateState.useAiTargetFolderPrediction}
              handleChange={(e) =>
                handleSettingsChanged({
                  useAiTargetFolderPrediction: e.currentTarget.checked,
                })
              }
            />
          </ListItemInput>
          <ListItemInput
            key="domain-mapping-configuration-link"
            marginTop="30px"
            label={getResource("interface.descriptionEmailDomainMappings")}
          >
            <Tooltip
              title={
                intermediateState.unmappedDomains > 0
                  ? getResource("interface.labelUnmappedDomains").replace(
                      "{Count}",
                      intermediateState.unmappedDomains.toFixed()
                    )
                  : ""
              }
              children={
                <IconButton
                  sx={{ marginRight: 1.5 }}
                  disabled={!dataSet.emailCleanerEnabled}
                  style={
                    dataSet.emailCleanerEnabled &&
                    intermediateState.unmappedDomains > 0
                      ? { border: `1px solid ${colors.error}` }
                      : { border: `1px solid transparent` }
                  }
                  onClick={() =>
                    navigate(
                      browserRoutes.emailDomainMapping.replace(
                        ":id",
                        intermediateState.settingsGuid
                      )
                    )
                  }
                >
                  <ChecklistRtlRounded />
                </IconButton>
              }
            />
          </ListItemInput>
        </List>
        <Grid2 display="flex" p={2} marginTop={5}>
          <InfoOutlined sx={{ paddingRight: 1, color: colors.lighter }} />
          {intermediateState.updatedAt && intermediateState.updatedBy && (
            <Typography>
              {getResource("interface.labelLastSettingsUpdateBy")
                .replace("{By}", intermediateState.updatedBy)
                .replace("{At}", intermediateState.updatedAt)}
            </Typography>
          )}
        </Grid2>
      </Grid2>
    </DetailsView>
  );
};

export default EmailCleanerInterfaceTab;
