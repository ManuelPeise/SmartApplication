import { Grid2, List, ListItemButton, Typography } from "@mui/material";
import React, { CSSProperties } from "react";
import { colors } from "src/_lib/colors";

export type VerticalTabListListItem = {
  key: number;
  icon?: React.ReactElement;
  title: string;
  subTitle: string;
  disabled?: boolean;
  onClick: () => void;
};

interface IProps {
  items: VerticalTabListListItem[];
  selectedTab: number;
}

export const verticalTablistItemStyle: CSSProperties = {
  width: 40,
  height: 40,
  color: colors.darkgray,
};

const VerticalTabListMenu: React.FC<IProps> = (props) => {
  const { items, selectedTab } = props;
  return (
    <List
      disablePadding
      sx={{
        minHeight: "100%",
        width: "100%",
        borderRight: `1px solid ${colors.lighter}`,
      }}
    >
      {items.map((item) => (
        <ListItemButton
          key={item.key}
          sx={{
            width: "100%",
          }}
          selected={item.key === selectedTab}
          disabled={item.disabled || item.key === selectedTab}
          onClick={item.onClick}
        >
          <Grid2 size={12} display="flex">
            <Grid2
              size={2}
              display="flex"
              justifyContent="center"
              alignItems="center"
            >
              {item?.icon}
            </Grid2>
            <Grid2 size={10} paddingLeft={2}>
              <Typography sx={{ fontSize: "1rem" }}>{item.title}</Typography>
              <Typography
                style={{
                  color: colors.typography.darkgray,
                  fontSize: ".9rem",
                }}
              >
                {item.subTitle}
              </Typography>
            </Grid2>
          </Grid2>
        </ListItemButton>
      ))}
    </List>
  );
};

export default VerticalTabListMenu;
