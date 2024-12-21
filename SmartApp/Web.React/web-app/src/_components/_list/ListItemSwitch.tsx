import { ListItem, ListItemText, Switch } from "@mui/material";
import React from "react";

interface IProps {
  label: string;
  enabled: boolean;
  onChange: (checked: boolean) => void;
}

const ListItemSwitch: React.FC<IProps> = (props) => {
  const { label, enabled, onChange } = props;

  const handleChange = React.useCallback(
    (event: React.ChangeEvent<HTMLInputElement>, checked: boolean) => {
      onChange(checked);
    },
    [onChange]
  );

  return (
    <ListItem
      divider
      sx={{ width: "100%", padding: 2 }}
      secondaryAction={
        <Switch checked={enabled} onChange={handleChange} color="success" />
      }
    >
      <ListItemText>{label}</ListItemText>
    </ListItem>
  );
};

export default ListItemSwitch;
