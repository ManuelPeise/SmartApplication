import {
  Box,
  Divider,
  List,
  ListItemButton,
  Paper,
  Typography,
} from "@mui/material";
import React, { PropsWithChildren } from "react";

export type SettingsListItem = {
  id: number;
  label: string;
  description?: string;
  selected: boolean;
  readonly: boolean;
  onSectionChanged: (id: number) => void;
  onMouseOver?: (id: number) => void;
};

interface IProps extends PropsWithChildren {
  listItems: SettingsListItem[];
  selectedItem: number;
}

const SettingsLayout: React.FC<IProps> = (props) => {
  const { children, listItems, selectedItem } = props;

  return (
    <Paper
      elevation={4}
      sx={{
        width: "100%",
        height: "100%",
        display: "flex",
        flexDirection: "row",
      }}
    >
      <Box display="flex" flexDirection="row" height="100%">
        <List sx={{ height: "100%", minWidth: "280px" }} disablePadding>
          {listItems.map((item) => (
            <ListItemButton
              sx={{
                display: "flex",
                flexDirection: "column",
                alignItems: "flex-start",
              }}
              key={item.id}
              disabled={item.selected}
              selected={item.id === selectedItem}
              onClick={item.onSectionChanged.bind(null, item.id)}
              onMouseDown={(e) => {
                item.onMouseOver && item.onMouseOver(item.id);
              }}
            >
              <Typography variant="body1" sx={{ padding: "2px 4px" }}>
                {item.label}
              </Typography>
              <Typography variant="body2" sx={{ padding: "2px 4px" }}>
                {item?.description ?? ""}
              </Typography>
            </ListItemButton>
          ))}
        </List>
      </Box>
      <Box>
        <Divider sx={{ height: "100%" }} orientation="vertical" />
      </Box>
      <Box id="settings-child-container" height="100%" width="100%">
        {children}
      </Box>
    </Paper>
  );
};

export default SettingsLayout;
