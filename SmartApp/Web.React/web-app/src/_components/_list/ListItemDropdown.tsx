import { ListItem, ListItemText } from "@mui/material";
import React from "react";
import { useI18n } from "src/_hooks/useI18n";
import EmailMappingActionDropdown from "src/_Stacks/_settings/_components/EmailMappingActionDropdown";

interface IProps {
  id: number;
  label: string;
  description: string;
  disabled?: boolean;
  value: number;
  onChange: (value: number) => void;
}

const ListItemDropdown: React.FC<IProps> = (props) => {
  const { id, description, value, onChange } = props;
  const { getResource } = useI18n();
  return (
    <ListItem
      divider
      sx={{ width: "100%", padding: 2 }}
      secondaryAction={
        <EmailMappingActionDropdown
          fullWidth
          itemId={id}
          // label={getResource("settings.labelCleanupAction")}
          value={value}
          onChange={onChange}
        />
      }
    >
      <ListItemText>{description}</ListItemText>
    </ListItem>
  );
};

export default ListItemDropdown;
