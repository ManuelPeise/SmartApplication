import { ListItem, ListItemText } from "@mui/material";
import React from "react";
import TextInput from "../_input/TextInput";

interface IProps {
  label: string;
  description: string;
  disabled?: boolean;
  isPassword?: boolean;
  value: string;
  onChange: (value: string) => void;
}

const ListItemTextField: React.FC<IProps> = (props) => {
  const { label, description, disabled, isPassword, value, onChange } = props;

  const handleChange = React.useCallback(
    (value: string) => {
      onChange(value);
    },
    [onChange]
  );

  return (
    <ListItem
      divider
      sx={{ width: "100%", padding: 2 }}
      disablePadding
      style={{
        display: "flex",
        flexDirection: "row",
        alignItems: "baseline",
      }}
      secondaryAction={
        <TextInput
          label={label}
          disabled={disabled}
          isPassword={isPassword}
          value={value}
          onChange={handleChange}
        />
      }
    >
      <ListItemText primaryTypographyProps={{}} primary={description} />
    </ListItem>
  );
};

export default ListItemTextField;
