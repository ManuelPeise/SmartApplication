import { ListItem, ListItemText } from "@mui/material";
import React from "react";
import FormButton from "../_buttons/FormButton";

interface IProps {
  label: string;
  description: string;
  disabled?: boolean;
  onAction: () => Promise<void> | void;
}

const ListItemButton: React.FC<IProps> = (props) => {
  const { label, description, disabled, onAction } = props;

  return (
    <ListItem
      divider
      sx={{ width: "100%", padding: 2 }}
      secondaryAction={
        <FormButton label={label} disabled={disabled} onAction={onAction} />
      }
    >
      <ListItemText>{description}</ListItemText>
    </ListItem>
  );
};

export default ListItemButton;
