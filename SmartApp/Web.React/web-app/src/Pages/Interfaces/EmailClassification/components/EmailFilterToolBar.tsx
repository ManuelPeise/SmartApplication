import React from "react";
import { EmailFilter } from "../types";
import {
  Box,
  Checkbox,
  FormControlLabel,
  Paper,
  Typography,
} from "@mui/material";
import { useI18n } from "src/_hooks/useI18n";
import FilterTextInput from "src/_components/Input/FilterTextInput";

interface IProps {
  filter: EmailFilter;
  itemsCount: number;
  handleFilterChanged: (partialFilter: Partial<EmailFilter>) => void;
}

const EmailFilterToolBar: React.FC<IProps> = (props) => {
  const { filter, itemsCount, handleFilterChanged } = props;
  const { getResource } = useI18n();

  return (
    <Paper sx={{ width: "inherit", height: "5rem" }} elevation={4}>
      <Box
        display="flex"
        flexDirection="row"
        gap={2}
        padding={1}
        justifyContent="flex-end"
        alignItems="center"
      >
        <Box width="20%" paddingRight={12}>
          <FilterTextInput
            fullwidth
            label={getResource("interface.labelAddressFilter")}
            value={filter.address}
            onChange={(value) => handleFilterChanged({ address: value })}
            handleClearFilterText={() => handleFilterChanged({ address: "" })}
          />
        </Box>
        <Box display="flex" alignContent="center" minWidth="10%">
          <FormControlLabel
            label={getResource("interface.labelHideHam")}
            labelPlacement="end"
            control={
              <Checkbox
                checked={filter.hideHam}
                onChange={(e) =>
                  handleFilterChanged({ hideHam: e.currentTarget.checked })
                }
              />
            }
          />
        </Box>
        <Box display="flex" alignContent="center" minWidth="10%">
          <FormControlLabel
            label={getResource("interface.labelHideSpam")}
            labelPlacement="end"
            control={
              <Checkbox
                checked={filter.hideSpam}
                onChange={(e) =>
                  handleFilterChanged({ hideSpam: e.currentTarget.checked })
                }
              />
            }
          />
        </Box>
        <Box display="flex" alignItems="center" minWidth="10%">
          <Typography sx={{ fontSize: "1.2rem" }}>
            {getResource("interface.labelAvailableItems").replace(
              "{Count}",
              itemsCount.toFixed(0)
            )}
          </Typography>
        </Box>
      </Box>
    </Paper>
  );
};

export default EmailFilterToolBar;
