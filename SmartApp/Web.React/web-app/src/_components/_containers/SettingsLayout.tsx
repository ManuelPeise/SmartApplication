import { Box, List, ListItem, Paper, Typography } from "@mui/material";
import React, { PropsWithChildren } from "react";
import { colors } from "src/_lib/colors";

export type SettingsListItem<T> = {
  id: number;
  index: number;
  label: string;
  selected: boolean;
  img?: string;
  model: T;
  onClick: React.Dispatch<React.SetStateAction<number>>;
};

interface IProps<T> extends PropsWithChildren {
  listitems: SettingsListItem<T>[];
}

function SettingsLayout<T>(props: IProps<T>) {
  const { children, listitems } = props;
  return (
    <Box
      display="flex"
      padding={{ xs: 0, sm: 0, md: 3, lg: 3, xl: 3 }}
      width="100%"
      flexDirection={{
        xs: "column",
        sm: "column",
        md: "row",
        lg: "row",
        xl: "row",
      }}
      justifyContent="flex-start"
      alignItems="center"
      gap={1}
    >
      <Paper
        sx={{
          width: {
            xs: "100vw",
            sm: "100vw",
            md: "500px",
            lg: "20%",
            xl: "20%",
          },
          height: {
            xs: "auto",
            sm: "auto",
            md: "100%",
            lg: "100%",
            xl: "100%",
          },
          padding: { xs: 0, md: 0 },
          borderRadius: 0,
        }}
      >
        <List disablePadding>
          {listitems?.map((item, key) => {
            return (
              <ListItem
                key={key}
                component="li"
                sx={{
                  padding: 2,
                  backgroundColor: item.selected
                    ? colors.background.lightgray
                    : "#fff",
                  pointerEvents: item.selected ? "none" : "all",
                  "&:hover": {
                    cursor: item.selected ? "not-allowed" : "pointer",
                  },
                  height: 50,
                }}
                onClick={item.onClick.bind(null, item.index)}
              >
                {item.img && (
                  <Box
                    component="img"
                    sx={{
                      padding: 2,
                      height: 30,
                      width: 30,
                    }}
                    alt="provider logo"
                    src={item.img}
                  />
                )}
                <Typography
                  sx={{
                    width: "16rem",
                    textAlign: "left",
                    textTransform: "capitalize",
                  }}
                >
                  {item.label}
                </Typography>
              </ListItem>
            );
          })}
        </List>
      </Paper>
      <Paper
        sx={{
          width: {
            xs: "100%",
            sm: "100%",
            md: "100%",
            lg: "100%",
            xl: "100%",
          },
          height: { sm: "100%" },
          padding: { xs: 0, md: 0 },
          borderRadius: 0,
        }}
      >
        {children}
      </Paper>
    </Box>
  );
}

export default SettingsLayout;
