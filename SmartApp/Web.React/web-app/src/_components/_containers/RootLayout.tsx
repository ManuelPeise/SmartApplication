import { Box } from "@mui/material";
import React, { PropsWithChildren } from "react";

const RootLayout: React.FC<PropsWithChildren> = (props) => {
  const { children } = props;

  return (
    <Box
      id="root-layout"
      display="flex"
      padding={{ xs: 0, sm: 0, md: 0, lg: 0, xl: 0 }}
      width="100vw"
      height="100vh"
    >
      {children}
    </Box>
  );
};

export default RootLayout;
