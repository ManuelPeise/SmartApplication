import React from "react";
import { Box, Checkbox, FormControlLabel, ListItem } from "@mui/material";
import { useI18n } from "src/_hooks/useI18n";
import TextInput from "src/_components/_input/TextInput";
import { FolderMappingEntry } from "../Types/emailCleanupTypes";

interface IProps {
  folderMappingEntry: FolderMappingEntry;
  handleUpdateMapping: (mapping: FolderMappingEntry) => void;
}

const FolderMappingListItem: React.FC<IProps> = (props) => {
  const { folderMappingEntry, handleUpdateMapping } = props;
  const { getResource } = useI18n();

  return (
    <ListItem disablePadding divider>
      <Box
        width="100%"
        display="flex"
        flexDirection="row"
        justifyContent="space-between"
        alignItems="center"
        padding="5px 1.5rem"
      >
        <FormControlLabel
          disableTypography
          label={getResource("settings.labelIsActive")}
          labelPlacement="end"
          control={
            <Checkbox
              checked={folderMappingEntry.isActive}
              onChange={(event) =>
                handleUpdateMapping({
                  ...folderMappingEntry,
                  isActive: event.currentTarget.checked,
                })
              }
            />
          }
        />
        <TextInput
          disabled
          label={getResource("settings.labelSourceFolder")}
          value={folderMappingEntry.source}
          onChange={(value) =>
            handleUpdateMapping({
              ...folderMappingEntry,
              source: value,
            })
          }
        />
        <TextInput
          disabled={!folderMappingEntry.isActive}
          label={getResource("settings.labelTargetFolder")}
          value={folderMappingEntry.target}
          onChange={(value) =>
            handleUpdateMapping({
              ...folderMappingEntry,
              target: value,
            })
          }
        />
      </Box>
    </ListItem>
  );
};

export default FolderMappingListItem;
