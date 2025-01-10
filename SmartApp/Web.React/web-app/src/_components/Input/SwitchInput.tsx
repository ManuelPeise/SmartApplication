import { Switch } from "@mui/material";
import React from "react";

interface IProps {
  checked: boolean;
  handleChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
  disabled?: boolean;
  checkedColor?: string;
}

const SwitchInput: React.FC<IProps> = (props) => {
  const { checked, checkedColor, disabled, handleChange } = props;

  return (
    <Switch
      checked={checked}
      sx={{
        "& .MuiSwitch-switchBase.Mui-checked + .MuiSwitch-track": {
          backgroundColor: "gray",
        },
        "& .MuiSwitch-switchBase": {
          color: "#fff",
        },
        "& .MuiSwitch-track": {
          backgroundColor: "gray",
        },
        "& .MuiSwitch-switchBase.Mui-checked": {
          color: checkedColor ?? "green",
        },
        "& .MuiSwitch-switchBase.Mui-checked.Mui-disabled": {
          color: "#ccffcc",
        },
      }}
      disabled={disabled}
      onChange={handleChange}
    />
  );
};

export default SwitchInput;
