import React from "react";
import { EmailCleanerSettings, FolderSettings } from "../EmailCleanerTypes";
import {
  FormControlLabel,
  Grid2,
  IconButton,
  Paper,
  Typography,
} from "@mui/material";
import SwitchInput from "src/_components/Input/SwitchInput";
import FormButton from "src/_components/Buttons/FormButton";
import { useI18n } from "src/_hooks/useI18n";
import { isEqual } from "lodash";
import { UpdateRounded } from "@mui/icons-material";

interface IProps {
  isLoading: boolean;
  folders: FolderSettings[];
  accountId: number;
  onClose: () => void;
  handleFoldersChanged: (partialState: Partial<EmailCleanerSettings>) => void;
  handleGetUpdatedFolders: (accountId: number) => Promise<FolderSettings[]>;
}

const InboxFolderConfiguration: React.FC<IProps> = (props) => {
  const {
    isLoading,
    accountId,
    folders,
    onClose,
    handleFoldersChanged,
    handleGetUpdatedFolders,
  } = props;
  const { getResource } = useI18n();
  const [folderSettings, setFolderSettings] =
    React.useState<FolderSettings[]>(folders);

  React.useEffect(() => {
    setFolderSettings(folders);
  }, [folders]);

  const isModified = React.useMemo(() => {
    return !isEqual(folders, folderSettings);
  }, [folders, folderSettings]);
  const handleChange = React.useCallback(
    (checked: boolean, index: number) => {
      const update = [
        ...folderSettings.map((f, i) =>
          i === index ? { ...f, isInbox: checked } : f
        ),
      ];

      setFolderSettings(update);
    },
    [folderSettings]
  );

  const onCancel = React.useCallback(() => {
    setFolderSettings(folders);
    onClose();
  }, [folders, onClose]);

  const onSave = React.useCallback(() => {
    handleFoldersChanged({ folderConfiguration: folderSettings });
    onClose();
  }, [folderSettings, onClose, handleFoldersChanged]);

  return (
    <Paper
      sx={{
        padding: 1,
        width: { sx: "100%", sm: "100%", md: "100%", lg: "30%", xl: "30%" },
      }}
    >
      <Grid2
        p={2}
        display="flex"
        justifyContent="space-between"
        alignItems="center"
      >
        <Typography variant="h6">
          {getResource("emailCleaner.captionFolderConfiguration")}
        </Typography>
        <IconButton
          size="small"
          disabled={isLoading}
          onClick={handleGetUpdatedFolders.bind(null, accountId)}
        >
          <UpdateRounded />
        </IconButton>
      </Grid2>
      <Grid2 paddingRight={2} paddingLeft={0} paddingBottom={4} spacing={2}>
        {folderSettings.map((f, index) => (
          <Grid2 key={f.folderId} padding={0.5}>
            <FormControlLabel
              sx={{ padding: 0, width: "100%" }}
              label={f.folderName}
              labelPlacement="start"
              slotProps={{
                typography: {
                  width: "100%",
                },
              }}
              control={
                <SwitchInput
                  checked={f.isInbox}
                  handleChange={(e) =>
                    handleChange(e.currentTarget.checked, index)
                  }
                />
              }
            />
          </Grid2>
        ))}
      </Grid2>
      <Grid2 display="flex" justifyContent="flex-end" gap={2}>
        <FormButton
          label={getResource("common.labelCancel")}
          disabled={isLoading}
          onAction={onCancel}
        />
        <FormButton
          label={getResource("common.labelOk")}
          disabled={!isModified}
          onAction={onSave}
        />
      </Grid2>
    </Paper>
  );
};

export default InboxFolderConfiguration;
