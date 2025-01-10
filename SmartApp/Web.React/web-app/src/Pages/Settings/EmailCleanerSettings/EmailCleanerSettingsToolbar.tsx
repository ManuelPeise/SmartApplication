import { Box, Paper, Typography } from "@mui/material";
import React from "react";
import Dropdown, { DropDownItem } from "src/_components/Input/Dropdown";
import { useI18n } from "src/_hooks/useI18n";
import { colors } from "src/_lib/colors";

interface IProps {
  accountDropdownItems: DropDownItem[];
  selectedAccount: number;
  accountDropdownDisabled: boolean;
  handleAccountChanged: React.Dispatch<React.SetStateAction<number>>;
}

const EmailCleanerSettingsToolbar: React.FC<IProps> = (props) => {
  const {
    accountDropdownItems,
    selectedAccount,
    accountDropdownDisabled,
    handleAccountChanged,
  } = props;
  const { getResource } = useI18n();

  return (
    <Box padding={2}>
      <Paper elevation={4}>
        <Box
          maxWidth="100%"
          padding={2}
          paddingLeft={4}
          paddingRight={4}
          display="flex"
          flexDirection="row"
          alignItems="baseline"
          justifyContent="space-between"
        >
          <Box>
            <Typography
              variant="h4"
              color={colors.typography.blue}
              fontStyle="italic"
              letterSpacing={0.2}
            >
              {getResource("settings.labelEmailCleanerSettings")}
            </Typography>
          </Box>
          <Box minWidth="300px" maxWidth="20%">
            <Dropdown
              fullWidth
              disabled={accountDropdownDisabled}
              items={accountDropdownItems}
              value={selectedAccount}
              onChange={handleAccountChanged}
            />
          </Box>
        </Box>
      </Paper>
    </Box>
  );
};

export default EmailCleanerSettingsToolbar;
