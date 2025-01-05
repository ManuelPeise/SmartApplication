import { TextField } from "@mui/material";
import React from "react";

interface IProps {
  label: string;
  value: string;
  inputMode: "numeric" | "decimal";
  fullwidth?: boolean;
  disabled?: boolean;
  onChange: (value: string) => void;
}

const NumberInput: React.FC<IProps> = (props) => {
  const { label, value, inputMode, fullwidth, disabled, onChange } = props;

  const handleChange = React.useCallback(
    (e: React.ChangeEvent<HTMLInputElement>) => {
      const inputValue = e.currentTarget.value;

      if (inputValue === "") {
        onChange("0");
      }

      if (!/^\d*$/.test(inputValue)) {
        return;
      }

      if (inputValue && parseInt(inputValue)) {
        onChange(inputValue);
      }
    },
    [onChange]
  );

  return (
    <TextField
      title="TEst"
      autoComplete="off"
      name="prevent-autofill-random"
      variant="standard"
      value={value === "0" ? "" : value}
      disabled={disabled}
      fullWidth={fullwidth}
      label={label}
      onChange={handleChange}
      slotProps={{
        input: {
          autoComplete: "off",
          inputProps: {
            inputMode: inputMode,
            style: { textAlign: "end" },
          },
        },
      }}
    />
  );
};

export default NumberInput;
