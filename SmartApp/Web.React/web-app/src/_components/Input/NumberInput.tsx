import { TextField } from "@mui/material";
import React from "react";

interface IProps {
  label: string;
  value: string;
  inputMode: "numeric" | "decimal";
  minValue?: number;
  maxValue?: number;
  fullwidth?: boolean;
  disabled?: boolean;
  onChange: (value: string) => void;
}

const NumberInput: React.FC<IProps> = (props) => {
  const {
    label,
    value,
    inputMode,
    fullwidth,
    minValue,
    maxValue,
    disabled,
    onChange,
  } = props;

  const handleChange = React.useCallback(
    (e: React.ChangeEvent<HTMLInputElement>) => {
      const inputValue = e.currentTarget.value;

      let isValid = true;

      if (inputValue === "") {
        onChange("0");
      }

      if (!/^\d*$/.test(inputValue)) {
        return;
      }

      if (
        (minValue && parseInt(inputValue) < minValue) ||
        (maxValue && parseInt(inputValue) > maxValue)
      ) {
        isValid = false;
      }

      if (inputValue && isValid && parseInt(inputValue)) {
        onChange(inputValue);
      }
    },
    [minValue, maxValue, onChange]
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
