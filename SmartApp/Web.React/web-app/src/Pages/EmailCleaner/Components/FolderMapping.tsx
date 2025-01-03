import React from "react";
import {
  EmailCleanupSettings,
  FolderMappingEntry,
  InboxConfiguration,
} from "../Types/emailCleanupTypes";
import { Box, IconButton, Tooltip } from "@mui/material";
import FolderMappingListItem from "./FolderMappingListItem";
import { AddRounded } from "@material-ui/icons";
import { useI18n } from "src/_hooks/useI18n";
import FormButton from "src/_components/_buttons/FormButton";
import { isEqual } from "lodash";

interface IProps {
  inboxConfiguration: InboxConfiguration;
  handleUpdateSettings: (
    partialState: Partial<EmailCleanupSettings>
  ) => Promise<void>;
  onClose: () => void;
}

const FolderMapping: React.FC<IProps> = (props) => {
  const { inboxConfiguration, handleUpdateSettings, onClose } = props;
  const { getResource } = useI18n();

  const [folderMappings, setFolderMappings] = React.useState<
    FolderMappingEntry[]
  >(inboxConfiguration.folderMappings);

  const handleUpdateMapping = React.useCallback(
    async (mapping: FolderMappingEntry) => {
      const mappings = [...folderMappings];

      const index =
        mappings.findIndex((x) => x.source === mapping.source) ?? null;

      if (index != null) {
        mappings[index] = mapping;
        setFolderMappings(mappings);
      }
    },
    [folderMappings]
  );

  const handleSaveMappings = React.useCallback(async () => {
    await handleUpdateSettings({
      inboxConfiguration: {
        ...inboxConfiguration,
        folderMappings: folderMappings,
      },
    }).then(() => {
      onClose();
    });
  }, [folderMappings, inboxConfiguration, handleUpdateSettings, onClose]);

  return (
    <Box width="100%">
      <Box
        display="flex"
        flexDirection="row"
        justifyContent="flex-end"
        paddingTop={2}
      >
        <Tooltip
          title={getResource("settings.labelAddNewFolder")}
          children={
            <IconButton size="small">
              <AddRounded />
            </IconButton>
          }
        />
      </Box>
      <Box>
        {folderMappings.map((item, index) => {
          return (
            <FolderMappingListItem
              key={index}
              folderMappingEntry={item}
              handleUpdateMapping={handleUpdateMapping}
            />
          );
        })}
      </Box>
      <Box
        sx={{
          display: "flex",
          flexDirection: "row",
          justifyContent: "flex-end",
          gap: 2,
          padding: 2,
        }}
      >
        <FormButton
          label={getResource("settings.labelCancel")}
          disabled={isEqual(inboxConfiguration.folderMappings, folderMappings)}
          onAction={() => setFolderMappings(inboxConfiguration.folderMappings)}
        />
        <FormButton
          label={getResource("settings.labelSave")}
          disabled={isEqual(inboxConfiguration.folderMappings, folderMappings)}
          onAction={handleSaveMappings}
        />
      </Box>
    </Box>
  );
};

export default FolderMapping;
