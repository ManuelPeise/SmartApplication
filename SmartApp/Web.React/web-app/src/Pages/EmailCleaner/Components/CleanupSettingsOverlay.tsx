import { ClearRounded } from "@material-ui/icons";
import { Backdrop, Box, IconButton, Paper, Typography } from "@mui/material";
import React from "react";
import {
  EmailCleanupSettings,
  InboxConfiguration,
} from "../Types/emailCleanupTypes";
import FolderMapping from "./FolderMapping";
import { useI18n } from "src/_hooks/useI18n";

interface IProps {
  show: boolean;
  inboxConfiguration: InboxConfiguration;
  handleUpdateSettings: (
    partialState: Partial<EmailCleanupSettings>
  ) => Promise<void>;
  onClose: () => void;
}

const CleanupSettingsOverlay: React.FC<IProps> = (props) => {
  const { show, inboxConfiguration, handleUpdateSettings, onClose } = props;
  const { getResource } = useI18n();

  if (!show) {
    return null;
  }

  return (
    <Backdrop
      open={show}
      sx={{
        height: "100%",
        width: "100%",
        position: "absolute",
        top: 0,
        left: 0,
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        backgroundColor: "rgba(0, 0, 0, 0.9)",
        opacity: 0.7,
      }}
    >
      <Box width="50%" height="50%" display="flex" justifyContent="center">
        <Box
          display="flex"
          justifyContent="flex-end"
          alignItems="baseline"
          flexDirection="row"
          gap={2}
          width={{
            xs: "100%",
            sm: "100%",
            md: "100%",
            lg: "100%",
            xl: "100%",
          }}
        >
          <Paper sx={{ width: "100%", padding: 2 }}>
            <Box
              width="100%"
              display="flex"
              justifyContent="space-between"
              alignItems="baseline"
            >
              <Typography variant="h6" sx={{ paddingLeft: 3 }}>
                {getResource("settings.labelCleanupSettings")}
              </Typography>
              <IconButton size="small" onClick={onClose}>
                <ClearRounded />
              </IconButton>
            </Box>
            <Box>
              <Typography>Ai settings</Typography>
            </Box>
            <Box>
              <Typography>Block list settings</Typography>
            </Box>
            <Box>
              <Typography>Test</Typography>
            </Box>
          </Paper>
        </Box>
      </Box>
    </Backdrop>
  );
};

export default CleanupSettingsOverlay;
