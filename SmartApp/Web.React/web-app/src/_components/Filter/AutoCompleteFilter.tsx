import { Autocomplete, TextField } from "@mui/material";
import React from "react";
import { DropDownItem } from "../Input/Dropdown";

interface IProps {
  componentKey: string;
  options: DropDownItem[];
  disabled?: boolean;
  fullWidth?: boolean;
  maxWidth?: number;
  minWidth?: number;
  placeHolder: string;
  handleChange: (id: number) => void;
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
    handleChange,
  } = props;

  const handleValueSelected = React.useCallback(
    (e: React.SyntheticEvent<Element, Event>, key: string) => {
      e.preventDefault();
      const id = options.find(
        (x) => x.label.toLowerCase() === key.toLowerCase()
      ).key;

      handleChange(id);
    },
    [handleChange, options]
  );

  return (
    <Autocomplete
      id={componentKey}
      disabled={disabled}
      disableClearable
      fullWidth={fullWidth}
      // noOptionsText={}
      sx={{
        minWidth: minWidth,
        maxWidth: maxWidth,
      }}
      onChange={handleValueSelected}
      options={options.map((opt) => opt.label)}
      renderInput={(params) => (
        <TextField
          {...params}
          variant="standard"
          slotProps={{
            input: {
              ...params.InputProps,
              placeholder: placeHolder,
              type: "search",
            },
          }}
        />
      )}
    />
  );
};

export default AutoCompleteFilter;
