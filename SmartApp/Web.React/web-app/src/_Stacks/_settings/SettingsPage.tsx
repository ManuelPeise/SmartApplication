import {
  Divider,
  Grid2,
  List,
  ListItemButton,
  ListItemText,
  Paper,
  Typography,
} from "@mui/material";
import React, { PropsWithChildren } from "react";
import { useI18n } from "src/_hooks/useI18n";

interface IProps extends PropsWithChildren {
  selectedTab: number;
  onSelectedTabChanged: (tabIndex: number) => void;
}

const SettingsPage: React.FC<IProps> = (props) => {
  const { children, selectedTab, onSelectedTabChanged } = props;
  const { getResource } = useI18n();
  return (
    <Grid2 padding={2} height="100%" width="100%">
      <Paper
        elevation={2}
        sx={{ height: "100%", display: "flex", flexDirection: "row" }}
      >
        <Grid2 width="20%">
          <List disablePadding>
            <ListItemButton
              selected={selectedTab === 0}
              disabled={selectedTab === 0}
              onClick={onSelectedTabChanged.bind(null, 0)}
            >
              <ListItemText>
                <Typography variant="h5">
                  {getResource("settings.labelEmailCleanupSettings")}
                </Typography>
              </ListItemText>
            </ListItemButton>
          </List>
        </Grid2>
        <Divider orientation="vertical" />
        <Grid2 width="80%">{children}</Grid2>
      </Paper>
    </Grid2>
  );
};

export default SettingsPage;
