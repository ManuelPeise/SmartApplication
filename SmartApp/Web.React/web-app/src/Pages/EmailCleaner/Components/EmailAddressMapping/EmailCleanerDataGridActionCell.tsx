import {
  OnlinePredictionRounded,
  SettingsBackupRestoreRounded,
  ShareRounded,
} from "@mui/icons-material";
import { Box, IconButton, Tooltip } from "@mui/material";
import React from "react";
import { useI18n } from "src/_hooks/useI18n";

interface IProps {
  padding: number;
  rowHasChanges: boolean;
  handleResetRow: () => void;
}

const EmailCleanerDataGridActionCell: React.FC<IProps> = (props) => {
  const { padding, rowHasChanges, handleResetRow } = props;
  const { getResource } = useI18n();
  return (
    <Box
      sx={{
        width: "100%",
        display: "flex",
        justifyContent: "center",
        flexDirection: "row",
        alignItems: "center",
        gap: 1,
        padding: padding,
        "&:hover": {
          cursor: "pointer",
        },
      }}
    >
      {/* share with ai */}
      <Tooltip
        sx={{
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
        }}
        title={getResource("emailCleaner.tooltipPredict")}
        children={
          <IconButton size="small" disabled>
            <OnlinePredictionRounded sx={{ width: 25, height: 25 }} />
          </IconButton>
        }
      />
      <Tooltip
        sx={{
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
        }}
        title={getResource("emailCleaner.toolTipShareWithAi")}
        children={
          <IconButton size="small" disabled>
            <ShareRounded sx={{ width: 20, height: 20 }} />
          </IconButton>
        }
      />
      <Tooltip
        sx={{
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
        }}
        title={getResource("emailCleaner.toolTipRevertChanges")}
        children={
          <IconButton
            size="small"
            disabled={!rowHasChanges}
            onClick={handleResetRow}
          >
            <SettingsBackupRestoreRounded sx={{ width: 25, height: 25 }} />
          </IconButton>
        }
      />
    </Box>
  );
};

export default EmailCleanerDataGridActionCell;
