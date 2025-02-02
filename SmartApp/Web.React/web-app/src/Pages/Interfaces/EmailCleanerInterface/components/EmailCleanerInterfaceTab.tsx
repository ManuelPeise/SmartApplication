import React from "react";
import DetailsView, { ButtonProps } from "src/_components/Layouts/DetailsView";
import { EmailCleanerSettings } from "../types";
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
import { InfoOutlined, SettingsRounded } from "@mui/icons-material";
import { browserRoutes } from "src/_lib/Router/RouterUtils";
import NoDataPlaceholder from "src/_components/Placeholders/NoDataPlaceholder";
import { useNavigate } from "react-router-dom";

interface IProps {
  tabindex: number;
  selectedTab: number;
  maxHeight: number;
  dataSet: EmailCleanerSettings;
  handleUpdateSettings: (model: EmailCleanerSettings) => Promise<void>;
  handleImportData: (accountId: number) => Promise<void>;
}

const EmailCleanerInterfaceTab: React.FC<IProps> = (props) => {
  const {
    tabindex,
    selectedTab,
    dataSet,
    maxHeight,
    handleUpdateSettings,
    handleImportData,
  } = props;
  const { getResource } = useI18n();
  const navigate = useNavigate();

  const [intermediateState, setIntermediateState] =
    React.useState<EmailCleanerSettings>(dataSet);

  const providerSettings = React.useMemo(() => {
    return emailProviderSettings.find((x) => x.type === dataSet.providerType);
  }, [dataSet]);

  const handleSettingsChanged = React.useCallback(
    (partialState: Partial<EmailCleanerSettings>) => {
      setIntermediateState({ ...intermediateState, ...partialState });
    },
    [intermediateState]
  );

  const onUpdate = React.useCallback(async () => {
    await handleUpdateSettings({
      ...dataSet,
      emailCleanerEnabled: intermediateState.emailCleanerEnabled,
      useScheduledEmailDataImport:
        intermediateState.useScheduledEmailDataImport,
    });
  }, [
    handleUpdateSettings,
    dataSet,
    intermediateState.emailCleanerEnabled,
    intermediateState.useScheduledEmailDataImport,
  ]);

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

  const additionalButtonProps = React.useMemo((): ButtonProps[] => {
    return [
      {
        label: getResource("interface.labelImportData"),
        disabled: dataSet.useScheduledEmailDataImport,
        onAction: handleImportData.bind(null, dataSet.accountId),
      },
    ];
  }, [
    dataSet.accountId,
    dataSet.useScheduledEmailDataImport,
    getResource,
    handleImportData,
  ]);

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
    <DetailsView
      saveCancelButtonProps={saveCancelButtonProps}
      additionalButtonProps={additionalButtonProps}
    >
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
        height={maxHeight}
        maxHeight={maxHeight}
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
                  useScheduledEmailDataImport: !e.currentTarget.checked
                    ? false
                    : intermediateState.useScheduledEmailDataImport,
                })
              }
            />
          </ListItemInput>
          <ListItemInput
            key="email-cleaner-scheduled-import"
            marginTop="30px"
            label={getResource("interface.descriptionScheduledDataImport")}
          >
            <SwitchInput
              disabled={!intermediateState.emailCleanerEnabled}
              checked={intermediateState.useScheduledEmailDataImport}
              handleChange={(e) =>
                handleSettingsChanged({
                  useScheduledEmailDataImport: e.currentTarget.checked,
                })
              }
            />
          </ListItemInput>
          <ListItemInput
            key="email-cleaner-classification-linl"
            marginTop="30px"
            label={getResource("interface.descriptionScheduledDataImport")}
          >
            <Tooltip
              title={getResource("interface.labelClassification")}
              children={
                <IconButton
                  size="medium"
                  sx={{ marginRight: 1 }}
                  onClick={() =>
                    navigate(
                      browserRoutes.emailClassification.replace(
                        ":id",
                        `${dataSet.accountId}`
                      )
                    )
                  }
                >
                  <SettingsRounded />
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
