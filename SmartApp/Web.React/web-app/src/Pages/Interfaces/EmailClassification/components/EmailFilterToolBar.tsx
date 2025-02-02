import React from "react";
import { EmailFilter } from "../types";
import {
  Box,
  Checkbox,
  FormControlLabel,
  Paper,
  Tooltip,
  Typography,
} from "@mui/material";
import { useI18n } from "src/_hooks/useI18n";
import AutoCompleteFilter from "src/_components/Filter/AutoCompleteFilter";
import { CategoryRounded } from "@mui/icons-material";

interface IProps {
  filter: EmailFilter;
  addressItems: string[];
  itemsCount: number;
  handleFilterChanged: (partialFilter: Partial<EmailFilter>) => void;
}

const EmailFilterToolBar: React.FC<IProps> = (props) => {
  const { filter, addressItems, itemsCount, handleFilterChanged } = props;
  const { getResource } = useI18n();

  return (
    <Paper
      sx={{
        display: "flex",
        alignItems: "center",
        width: "inherit",
        height: "5rem",
      }}
      elevation={4}
    >
      <Box
        width="100%"
        display="flex"
        flexDirection="row"
        gap={2}
        padding={1}
        justifyContent="space-between"
        alignItems="center"
      >
        <Box width="25%" paddingLeft={4}>
          <AutoCompleteFilter
            componentKey="email-address-filter"
            options={addressItems}
            value={filter.address}
            placeHolder={getResource("interface.labelAddressFilter")}
            handleChange={(e) =>
              handleFilterChanged({
                address: e,
              })
            }
          />
        </Box>
        <Box
          display="flex"
          width="75%"
          flexDirection="row"
          justifyContent="flex-end"
          gap={2}
          paddingRight={4}
        >
          <Box display="flex" alignContent="center" minWidth="10%">
            <FormControlLabel
              label={getResource("interface.labelModifyEntireFilterResult")}
              labelPlacement="end"
              control={
                <Checkbox
                  disabled={filter.address === ""}
                  checked={filter.modifyEntireFilterResult}
                  onChange={(e) =>
                    handleFilterChanged({
                      modifyEntireFilterResult: e.currentTarget.checked,
                    })
                  }
                />
              }
            />
          </Box>
          <Box display="flex" alignContent="center" minWidth="10%">
            <FormControlLabel
              label={getResource("interface.labelHideHam")}
              labelPlacement="end"
              control={
                <Checkbox
                  disabled={filter.hideSpam}
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
                  disabled={filter.hideHam}
                  checked={filter.hideSpam}
                  onChange={(e) =>
                    handleFilterChanged({ hideSpam: e.currentTarget.checked })
                  }
                />
              }
            />
          </Box>
          <Box display="flex" alignItems="center" minWidth="5%">
            <Tooltip
              title={getResource("interface.labelCategories").replace(
                "{Count}",
                itemsCount.toFixed(0)
              )}
              children={
                <Typography sx={{ position: "relative", fontSize: "1.2rem" }}>
                  <CategoryRounded />
                  <span style={{ position: "absolute", bottom: 10 }}>
                    {itemsCount.toFixed(0)}
                  </span>
                </Typography>
              }
            />
          </Box>
        </Box>
      </Box>
    </Paper>
  );
};

export default EmailFilterToolBar;
