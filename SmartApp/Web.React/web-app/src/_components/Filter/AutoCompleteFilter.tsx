import { ClearRounded } from "@mui/icons-material";
import { Autocomplete, IconButton, TextField } from "@mui/material";
import React from "react";

interface IProps {
  componentKey: string;
  options: string[];
  disabled?: boolean;
  fullWidth?: boolean;
  maxWidth?: number;
  minWidth?: number;
  placeHolder: string;
  value: string;
  handleChange: (id: string) => void;
}

const AutoCompleteFilter: React.FC<IProps> = (props) => {
  const {
    componentKey,
    disabled,
    fullWidth,
    minWidth,
    maxWidth,
    options,
    placeHolder,
    value,
    handleChange,
  } = props;

  const handleValueSelected = React.useCallback(
    (e: React.SyntheticEvent<Element, Event>, key: string, reason: string) => {
      e.preventDefault();
      handleChange(key);
    },
    [handleChange]
  );

  return (
    <Autocomplete
      id={componentKey}
      disabled={disabled}
      disableClearable
      fullWidth={fullWidth}
      value={value}
      sx={{
        minWidth: minWidth,
        maxWidth: maxWidth,
        alignContent: "end",
      }}
      clearOnEscape
      onChange={handleValueSelected}
      options={options.map((opt) => opt)}
      renderInput={(params) => (
        <TextField
          {...params}
          variant="standard"
          slotProps={{
            input: {
              ...params.InputProps,
              placeholder: placeHolder,
              // type: value "search",
              startAdornment: value !== "" && (
                <IconButton
                  sx={{ width: 10, height: 10, margin: "8px" }}
                  onClick={() => handleChange("")}
                >
                  <ClearRounded />
                </IconButton>
              ),
            },
          }}
        />
      )}
    />
  );
};

export default AutoCompleteFilter;
