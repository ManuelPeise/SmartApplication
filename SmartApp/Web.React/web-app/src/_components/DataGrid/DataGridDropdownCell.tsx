import { Box, MenuItem, Select } from "@mui/material";
import React from "react";
import { DropDownItem } from "../Input/Dropdown";

interface IProps {
  rowId: number;
  padding: number;
  value: number;
  items: DropDownItem[];
  disabled: boolean;
  handleChange: (modelId: number, value: number) => void;
}

const DataGridDropdownCell: React.FC<IProps> = (props) => {
  const { rowId, padding, value, disabled, items, handleChange } = props;

  return (
    <Box
      sx={{
        width: "100%",
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        padding: padding,
        marginBottom: -0.7,
        "&:hover": {
          cursor: "pointer",
        },
      }}
    >
      <Select
        fullWidth
        variant="standard"
        disabled={disabled}
        value={value}
        onChange={(e) => handleChange(rowId, e.target.value as number)}
      >
        {items.map((item) => (
          <MenuItem key={item.key} value={item.key} disabled={item.disabled}>
            {item.label}
          </MenuItem>
        ))}
      </Select>
    </Box>
  );
};

export default DataGridDropdownCell;
