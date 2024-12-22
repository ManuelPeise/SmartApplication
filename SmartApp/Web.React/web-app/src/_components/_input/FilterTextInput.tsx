import { DeleteOutlineRounded } from "@material-ui/icons";
import { IconButton, TextField } from "@mui/material";
import React from "react";

interface IProps {
  label: string;
  value: string;
  fullwidth?: boolean;
  disabled?: boolean;
  onChange: (value: string) => void;
  handleClearFilterText: () => void;
}

const FilterTextInput: React.FC<IProps> = (props) => {
  const { label, value, fullwidth, disabled, onChange, handleClearFilterText } =
    props;

  const handleChange = React.useCallback(
    (e: React.ChangeEvent<HTMLInputElement>) => {
      onChange(e.currentTarget.value);
    },
    [onChange]
  );

  return (
    <TextField
      autoComplete="off"
      variant="standard"
      value={value}
      disabled={disabled}
      type={"text"}
      fullWidth={fullwidth}
      label={label}
      onChange={handleChange}
      slotProps={{
        input: {
          endAdornment:
            value !== "" ? (
              <IconButton
                sx={{
                  width: "30px",
                  height: "30px",
                }}
              >
                <DeleteOutlineRounded
                  style={{
                    width: "30px",
                    height: "30px",
                  }}
                  onClick={handleClearFilterText}
                />
              </IconButton>
            ) : null,
          autoComplete: "off",
          inputProps: {
            autoComplete: "off",
          },
        },
      }}
    />
  );
};

export default FilterTextInput;
