import { Box } from "@mui/material";
import React, { PropsWithChildren } from "react";
import AppHeaderBar from "src/_components/_toolbars/AppHeaderBar";
import { colors } from "../colors";
import AppDrawer from "./AppDrawer";

const PageLayout: React.FC<PropsWithChildren> = (props) => {
  const { children } = props;
  const [sideMenuOpen, setSideMenuOpen] = React.useState<boolean>(false);

  return (
    <Box
      width="100vw"
      height="100vh"
      padding={0}
      margin={0}
      display="flex"
      flexDirection="column"
    >
      <Box
        display="flex"
        flexDirection="column"
        height="100%"
        bgcolor={colors.background}
      >
        <AppHeaderBar onOpenSideMenu={setSideMenuOpen} />
        <AppDrawer
          open={sideMenuOpen}
          onClose={setSideMenuOpen.bind(null, false)}
        />
        <Box width="100%" height="100%">
          {children}
        </Box>
      </Box>
    </Box>
  );
};

export default PageLayout;
