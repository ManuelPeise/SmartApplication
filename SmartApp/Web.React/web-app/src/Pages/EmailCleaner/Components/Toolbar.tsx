import { MapRounded, SettingsRounded } from "@material-ui/icons";
import { Box, IconButton, Paper, Tooltip } from "@mui/material";
import React from "react";
import Dropdown, { DropDownItem } from "src/_components/_input/Dropdown";
import { useI18n } from "src/_hooks/useI18n";
import {
  EmailCleanupSettings,
  FolderMappingEntry,
} from "../Types/emailCleanupTypes";

interface IProps {
  settingIndex: number;
  settings: EmailCleanupSettings[];
  selectedFolderMapping: FolderMappingEntry;
  handleSelectedSettingsIndexChanged: (index: number) => void;
  handleSelectedFolderChanged: (index: number) => void;
  handleShowFolderMappingOverlay: () => void;
  handleShowCleanupSettingsOverlay: () => void;
}

const Toolbar: React.FC<IProps> = (props) => {
  const {
    settingIndex,
    settings,
    selectedFolderMapping,
    handleSelectedSettingsIndexChanged,
    handleSelectedFolderChanged,
    handleShowFolderMappingOverlay,
    handleShowCleanupSettingsOverlay,
  } = props;
  const { getResource } = useI18n();

  const dropdownItems = React.useMemo((): DropDownItem[] => {
    return settings.map((setting, index) => {
      return {
        key: index,
        label: setting.accountSettings.emailAddress,
        disabled: index === settingIndex,
      };
    });
  }, [settingIndex, settings]);

  const inboxDropdownItems = React.useMemo((): DropDownItem[] => {
    return settings[settingIndex].inboxConfiguration.folderMappings.map(
      (folder, index) => {
        return {
          key: index,
          label: folder.source,
          disabled:
            folder.source === selectedFolderMapping.source || !folder.isActive,
        };
      }
    );
  }, [settingIndex, settings, selectedFolderMapping]);

  return (
    <Paper sx={{ padding: 2 }}>
      <Box
        display="flex"
        flexDirection="row"
        alignItems="baseline"
        alignContent="space-between"
        gap={2}
      >
        <Box
          width={{ xs: "50%", sm: "25%", md: "25%", lg: "15%", xl: "15%" }}
          display="flex"
          justifyContent="flex-start"
        >
          <Dropdown
            minWidth={200}
            items={dropdownItems}
            value={settingIndex}
            onChange={(index) => handleSelectedSettingsIndexChanged(index)}
            fullWidth
            disabled={settings?.length === 0}
          />
        </Box>
        <Box>
          <Dropdown
            minWidth={200}
            items={inboxDropdownItems}
            value={
              inboxDropdownItems.find(
                (x) => x.label === selectedFolderMapping.source
              ).key
            }
            onChange={(index) => handleSelectedFolderChanged(index)}
            fullWidth
            disabled={settings?.length === 0}
          />
        </Box>
        <Box
          width={{ xs: "50%", sm: "75%", md: "75%", lg: "85%", xl: "85%" }}
          display="flex"
          justifyContent="flex-end"
          gap={2}
        >
          <IconButton size="small" onClick={handleShowFolderMappingOverlay}>
            <Tooltip
              title={getResource("settings.labelFolderMapping")}
              children={<MapRounded />}
            />
          </IconButton>
          <IconButton size="small" onClick={handleShowCleanupSettingsOverlay}>
            <Tooltip
              title={getResource("settings.labelSettings")}
              children={<SettingsRounded />}
            />
          </IconButton>
        </Box>
      </Box>
    </Paper>
  );
};

export default Toolbar;
