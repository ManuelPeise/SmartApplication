import React from "react";
import { Box, IconButton, ListItem, Tooltip } from "@mui/material";
import { emailProviderSettings } from "src/_lib/Settings/EmailProviderSettings";
import { ModeRounded, VisibilityRounded } from "@mui/icons-material";
import { EmailProviderTypeEnum } from "src/_lib/_enums/EmailProviderTypeEnum";

interface IProps {
  providerType: EmailProviderTypeEnum;
  toggleFormModeDisabled: boolean;
  toggleLabel: string;
  formMode: "view" | "edit";
  toggleMode: (mode: "view" | "edit") => void;
}

const EmailAccountHeaderListItem: React.FC<IProps> = (props) => {
  const {
    providerType,
    toggleFormModeDisabled,
    toggleLabel,
    formMode,
    toggleMode,
  } = props;

  const providerSettings = React.useMemo(() => {
    return emailProviderSettings.find((x) => x.type === providerType);
  }, [providerType]);

  return (
    <ListItem divider sx={{ width: "100%" }}>
      <Box
        display="flex"
        flexDirection="row"
        alignItems="flex-end"
        gap={2}
        justifyContent="space-between"
        width="100%"
        padding={2}
      >
        <Box display="flex" flexDirection="row" alignItems="flex-end" gap={4}>
          <Box
            component="img"
            src={providerSettings.imageSrc}
            alt="provider logo"
            sx={{ width: "50px", height: "50px" }}
          />
        </Box>
        <Box
          width="50%"
          display="flex"
          flexDirection="row"
          alignItems="flex-end"
          justifyContent="flex-end"
          gap={2}
        >
          <Tooltip
            title={toggleLabel}
            children={
              <IconButton
                size="small"
                disabled={toggleFormModeDisabled}
                onClick={toggleMode.bind(
                  null,
                  formMode === "view" ? "edit" : "view"
                )}
                children={
                  formMode === "view" ? <ModeRounded /> : <VisibilityRounded />
                }
              />
            }
          />
        </Box>
      </Box>
    </ListItem>
  );
};

export default EmailAccountHeaderListItem;
