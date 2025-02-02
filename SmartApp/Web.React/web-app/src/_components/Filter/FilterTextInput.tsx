import { SearchRounded } from "@mui/icons-material";
import { TextField } from "@mui/material";
import React from "react";

interface IProps {
  label: string;
  filterText: string;
  disabled?: boolean;
  fullWidth?: boolean;
  onChange: (value: string) => void;
}

const FilterTextInput: React.FC<IProps> = (props) => {
  const { filterText, label, disabled, fullWidth, onChange } = props;

  const handleChange = React.useCallback(
    (e: React.ChangeEvent<HTMLInputElement>) => {
      onChange(e.currentTarget.value);
    },
    [onChange]
  );

  return (
    <TextField
      type="text"
      label={label}
      fullWidth={fullWidth}
      disabled={disabled}
      value={filterText}
      variant="standard"
      onChange={handleChange}
      slotProps={{
        input: {
          endAdornment: <SearchRounded />,
        },
      }}
    />
  );
};

export default FilterTextInput;
