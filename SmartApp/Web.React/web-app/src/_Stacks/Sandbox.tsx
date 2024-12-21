import { Box, List, Paper } from "@mui/material";
import React, { PropsWithChildren } from "react";

interface IProps extends PropsWithChildren {}

const SandBox: React.FC<IProps> = (props) => {
  const { children } = props;
  return (
    <Box
      display="flex"
      padding={{ xs: 0, sm: 0, md: 1, lg: 2, xl: 2 }}
      width="100vw"
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
            xs: "100%",
            sm: "100vw",
            md: "100vw",
            lg: "20%",
            xl: "20%",
          },
          height: { sm: "100%" },
          padding: { xs: 0, md: 0 },
        }}
      >
        <List disablePadding></List>
      </Paper>
      <Paper
        sx={{
          width: {
            xs: "100vw",
            sm: "100vw",
            md: "100vw",
            lg: "80%",
            xl: "80%",
          },
          height: { sm: "100%" },
          padding: { xs: 0, md: 0 },
        }}
      >
        {children}
      </Paper>
    </Box>
  );
};

export default SandBox;
