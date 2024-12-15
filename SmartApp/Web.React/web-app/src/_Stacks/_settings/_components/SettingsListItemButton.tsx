import { Grid2, ListItem, Typography } from "@mui/material";
import React from "react";
import FormButton from "src/_components/_buttons/FormButton";
import { colors } from "src/_lib/colors";

interface IProps {
  disabled: boolean;
  btnLabel: string;
  description: string;
  onAction: () => void;
}

const SettingsListItemButton: React.FC<IProps> = (props) => {
  const { disabled, btnLabel, description, onAction } = props;

  return (
    <ListItem divider sx={{ width: "100%", padding: 0 }}>
      <Grid2
        display="flex"
        justifyContent="space-between"
        alignItems="baseline"
        width="100%"
        padding={1}
        paddingLeft={4}
        paddingRight={4}
      >
        <Grid2>
          <Typography
            variant="h6"
            color={colors.typography.darkgray}
            fontStyle="italic"
          >
            {description}
          </Typography>
        </Grid2>
        <Grid2>
          <FormButton
            disabled={disabled}
            label={btnLabel}
            onAction={onAction}
          />
        </Grid2>
      </Grid2>
    </ListItem>
  );
};

export default SettingsListItemButton;
