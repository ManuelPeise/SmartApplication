import { Grid2 } from "@mui/material";
import React, { PropsWithChildren } from "react";
import AppHeaderBar from "src/_components/Toolbars/AppHeaderBar";
import { colors } from "../colors";
import AppDrawer from "./AppDrawer";

const PageLayout: React.FC<PropsWithChildren> = (props) => {
  const { children } = props;
  const [sideMenuOpen, setSideMenuOpen] = React.useState<boolean>(false);

  return (
    <Grid2
      id="page-layout-container"
      container
      height="100vh"
      width="100%"
      display="flex"
      flexDirection="column"
      bgcolor={colors.background}
    >
      <AppHeaderBar onOpenSideMenu={setSideMenuOpen} />
      <AppDrawer
        open={sideMenuOpen}
        onClose={setSideMenuOpen.bind(null, false)}
      />
      <Grid2
        id="page-layout-child-container"
        width="100%"
        height="94vh"
        container
        size={12}
      >
        {children}
      </Grid2>
    </Grid2>
  );
};

export default PageLayout;
