import {
  FormControl,
  InputLabel,
  MenuItem,
  Select,
  SelectChangeEvent,
} from "@mui/material";
import React from "react";
import { FilterDropdownItem } from "src/_lib/_types/filter";

interface IProps {
  selectedItem: FilterDropdownItem;
  items: FilterDropdownItem[];
  label: string;
  disabled?: boolean;
  fullWidth?: boolean;
  maxHeight?: number;
  onChange: (value: number) => void;
}

const FilterDropdown: React.FC<IProps> = (props) => {
  const {
    selectedItem,
    items,
    label,
    disabled,
    fullWidth,
    maxHeight,
    onChange,
  } = props;

  const handleChange = React.useCallback(
    (e: SelectChangeEvent<number>) => {
      onChange(e.target.value as number);
    },
    [onChange]
  );

  return (
    <FormControl fullWidth={fullWidth}>
      <InputLabel>{label}</InputLabel>
      <Select
        fullWidth={fullWidth}
        disabled={disabled}
        value={selectedItem?.value ?? -1}
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
            <MenuItem key={key} disabled={item.disabled} value={item.value}>
              {item.label}
            </MenuItem>
          );
        })}
      </Select>
    </FormControl>
  );
};

export default FilterDropdown;
