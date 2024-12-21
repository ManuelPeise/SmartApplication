import { TextField } from "@mui/material";
import React from "react";

interface IProps {
  label: string;
  value: string;
  isPassword?: boolean;
  fullwidth?: boolean;
  disabled?: boolean;
  onChange: (value: string) => void;
}

const TextInput: React.FC<IProps> = (props) => {
  const { label, value, isPassword, fullwidth, disabled, onChange } = props;

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
      type={isPassword ? "password" : "text"}
      fullWidth={fullwidth}
      label={label}
      onChange={handleChange}
      slotProps={{
        input: {
          autoComplete: "off",
          inputProps: {
            autoComplete: "off",
          },
        },
      }}
    />
  );
};

export default TextInput;
