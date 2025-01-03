import React, { PropsWithChildren } from "react";
import { EmailCleanupSettings, SpamReport } from "./Types/emailCleanupTypes";
import LoadingIndicator from "src/_components/_loading/LoadingIndicator";
import { Box } from "@mui/material";
import Toolbar from "./Components/Toolbar";
import InboxTable from "./Components/InboxTable";
import FolderMappingOverlay from "./Components/FolderMappingOverlay";
import { useEmailCleanerStore } from "./Store/useEmailCleanerStore";
import CleanupSettingsOverlay from "./Components/CleanupSettingsOverlay";

interface IProps extends PropsWithChildren {
  isLoading: boolean;
  showConnectionScreen: boolean;
  settings: EmailCleanupSettings[];
  handleUpdateSettings: (settings: EmailCleanupSettings) => Promise<void>;
  handleReportAiTrainingData: (reportModel: SpamReport) => Promise<void>;
}

const EmailCleaner: React.FC<IProps> = (props) => {
  const {
    isLoading,
    settings,
    handleUpdateSettings,
    handleReportAiTrainingData,
  } = props;

  const {
    emailSettings,
    selectedEmailSettings,
    settingsIndex,
    selectedFolderMapping,
    handleSelectedSettingsIndexChanged,
    handleSelectedFolderChanged,
    handleUpdateState,
  } = useEmailCleanerStore(settings);

  const [showFolderMappingOverlay, setShowFolderMappingOverlay] =
    React.useState<boolean>(false);
  const [showCleanUpSettingsOverlay, setShowCleanUpSettingsOverlay] =
    React.useState<boolean>(false);

  const handleUpdateEmailSettings = React.useCallback(
    async (partialState: Partial<EmailCleanupSettings>) => {
      const settingsUpdate: EmailCleanupSettings = {
        ...selectedEmailSettings,
        ...partialState,
      };

      await handleUpdateSettings(settingsUpdate).then(() => {
        handleUpdateState(settingsUpdate.id, partialState);
      });
    },
    [selectedEmailSettings, handleUpdateSettings, handleUpdateState]
  );

  return (
    <Box
      width="100%"
      display="flex"
      flexDirection="column"
      justifyContent="flex-start"
      padding={2}
      gap={2}
    >
      <LoadingIndicator isLoading={isLoading} />
      <Toolbar
        settingIndex={settingsIndex}
        settings={emailSettings}
        selectedFolderMapping={selectedFolderMapping}
        handleSelectedSettingsIndexChanged={handleSelectedSettingsIndexChanged}
        handleSelectedFolderChanged={handleSelectedFolderChanged}
        handleShowFolderMappingOverlay={setShowFolderMappingOverlay.bind(
          null,
          true
        )}
        handleShowCleanupSettingsOverlay={setShowCleanUpSettingsOverlay.bind(
          null,
          true
        )}
      />
      {/* table with mail informations */}
      <Box width="100%">
        <InboxTable
          settings={selectedEmailSettings}
          selectedFolderMapping={selectedFolderMapping}
          stickyHeader={
            !showFolderMappingOverlay && !showCleanUpSettingsOverlay
          }
          handleUpdateState={handleUpdateEmailSettings}
          handleReportAiTrainingData={handleReportAiTrainingData}
        />
      </Box>
      <FolderMappingOverlay
        show={showFolderMappingOverlay}
        inboxConfiguration={selectedEmailSettings.inboxConfiguration}
        handleUpdateSettings={handleUpdateEmailSettings}
        onClose={setShowFolderMappingOverlay.bind(null, false)}
      />
      <CleanupSettingsOverlay
        show={showCleanUpSettingsOverlay}
        inboxConfiguration={selectedEmailSettings.inboxConfiguration}
        handleUpdateSettings={handleUpdateEmailSettings}
        onClose={setShowCleanUpSettingsOverlay.bind(null, false)}
      />
    </Box>
  );
};

export default EmailCleaner;
