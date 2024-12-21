import { ListItem, ListItemText } from "@mui/material";
import React from "react";
import NumberInput from "../_input/NumberInput";

interface IProps {
  label: string;
  description: string;
  disabled?: boolean;
  value: number;
  inputMode: "numeric" | "decimal";
  onChange: (value: string) => void;
}

const ListItemNumberField: React.FC<IProps> = (props) => {
  const { label, description, disabled, value, inputMode, onChange } = props;

  return (
    <ListItem
      divider
      sx={{ width: "100%", padding: 2 }}
      secondaryAction={
        <NumberInput
          disabled={disabled}
          label={label}
          value={value?.toString()}
          inputMode={inputMode}
          onChange={onChange}
        />
      }
    >
      <ListItemText>{description}</ListItemText>
    </ListItem>
  );
};

export default ListItemNumberField;
