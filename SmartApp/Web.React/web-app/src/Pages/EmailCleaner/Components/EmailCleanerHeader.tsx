import { AddRounded } from "@mui/icons-material";
import {
  Box,
  Grid2,
  IconButton,
  Paper,
  Tooltip,
  Typography,
} from "@mui/material";
import React from "react";
import Dropdown, { DropDownItem } from "src/_components/Input/Dropdown";
import { useI18n } from "src/_hooks/useI18n";
import { colors } from "src/_lib/colors";
import { EmailCleanerOverlayStatus } from "../EmailCleanerTypes";

interface IProps {
  accountNames: string[];
  selectedAccountIndex: number;
  overlayState: EmailCleanerOverlayStatus;
  handleOverlayStateChanged: (
    partialState: Partial<EmailCleanerOverlayStatus>
  ) => void;
  handleSelectedAccountChanged: (index: number) => void;
}

const EmailCleanerHeader: React.FC<IProps> = (props) => {
  const {
    accountNames,
    selectedAccountIndex,
    overlayState,
    handleOverlayStateChanged,
    handleSelectedAccountChanged,
  } = props;

  const { getResource } = useI18n();

  const accountListItems = React.useMemo((): DropDownItem[] => {
    const items: DropDownItem[] = [];

    items.push({
      key: -1,
      label: getResource("emailCleaner.labelSelectAccount"),
      disabled: true,
    });

    accountNames.forEach((name, index) => {
      items.push({
        key: index,
        label: name,
        disabled: false, // o.selectedAccountIndex === index + 1,
      });
    });
    return items;
  }, [getResource, accountNames]);

  return (
    <Grid2 id="email-cleaner-header" size={12}>
      <Paper sx={{ minWidth: "900px", paddingTop: 2, paddingBottom: 2 }}>
        <Box
          display="flex"
          alignItems="baseline"
          justifyContent="space-between"
        >
          <Box paddingLeft={4} width="80%" minWidth="30rem">
            <Typography
              variant="h4"
              fontStyle="italic"
              color={colors.typography.blue}
            >
              {getResource("emailCleaner.captionEmailCleaner")}
            </Typography>
          </Box>
          <Box
            display="flex"
            flexDirection="row"
            justifyContent="space-between"
            alignItems="baseline"
            paddingRight={4}
            width="20%"
            minWidth="15rem"
          >
            <Box width="80%" minWidth="12rem">
              <Dropdown
                fullWidth
                disabled={!accountNames?.length}
                items={accountListItems}
                value={selectedAccountIndex}
                onChange={(index) => handleSelectedAccountChanged(index)}
              />
            </Box>
            <Box>
              <IconButton
                size="small"
                disabled={overlayState.addAccountOverlayOpen}
                onClick={handleOverlayStateChanged.bind(null, {
                  addAccountOverlayOpen: true,
                })}
              >
                <Tooltip
                  title={getResource("emailCleaner.labelAddAccount")}
                  children={
                    <AddRounded sx={{ color: colors.typography.blue }} />
                  }
                />
              </IconButton>
            </Box>
          </Box>
        </Box>
      </Paper>
    </Grid2>
  );
};

export default EmailCleanerHeader;
