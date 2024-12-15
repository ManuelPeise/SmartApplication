import { SearchRounded } from "@material-ui/icons";
import { TextField } from "@mui/material";
import React from "react";

interface IProps {
  label: string;
  filterText: string;
  disabled?: boolean;
  onChange: (value: string) => void;
}

const FilterTextInput: React.FC<IProps> = (props) => {
  const { filterText, label, disabled, onChange } = props;

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
