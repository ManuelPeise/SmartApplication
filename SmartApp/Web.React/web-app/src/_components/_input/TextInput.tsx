import { TextField } from "@mui/material";
import React from "react";

interface IProps {
  label: string;
  value: string;
  isPassword?: boolean;
  fullwidth?: boolean;
  onChange: (value: string) => void;
}

const TextInput: React.FC<IProps> = (props) => {
  const { label, value, isPassword, fullwidth, onChange } = props;

  const handleChange = React.useCallback(
    (e: React.ChangeEvent<HTMLInputElement>) => {
      onChange(e.currentTarget.value);
    },
    [onChange]
  );

  return (
    <TextField
      variant="standard"
      value={value}
      type={isPassword ? "password" : "text"}
      fullWidth={fullwidth}
      label={label}
      onChange={handleChange}
      slotProps={{
        input: { autoComplete: "off" },
      }}
    />
  );
};

export default TextInput;
