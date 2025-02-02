import { MenuItem, Select, SelectChangeEvent } from "@mui/material";
import React from "react";

export type DropDownItem = {
  key: number;
  label: string;
  disabled?: boolean;
};

interface IProps {
  disabled?: boolean;
  fullWidth?: boolean;
  minWidth?: number;
  items: DropDownItem[];
  value: number;
  maxHeight?: number;
  onChange: (value: number) => void;
}

const Dropdown: React.FC<IProps> = (props) => {
  const { disabled, items, fullWidth, minWidth, value, maxHeight, onChange } =
    props;

  const handleChange = React.useCallback(
    (e: SelectChangeEvent<number>) => {
      onChange(e.target.value as number);
    },
    [onChange]
  );

  return (
    <Select
      disabled={disabled}
      fullWidth={fullWidth}
      sx={{ minWidth: minWidth }}
      value={value}
      MenuProps={{
        style: {
          maxHeight: maxHeight,
        },
      }}
      variant="standard"
      onChange={handleChange}
    >
      {items.map((item, key) => {
        return (
          <MenuItem key={key} value={item.key} disabled={item.disabled}>
            {item.label}
          </MenuItem>
        );
      })}
    </Select>
  );
};

export default Dropdown;
