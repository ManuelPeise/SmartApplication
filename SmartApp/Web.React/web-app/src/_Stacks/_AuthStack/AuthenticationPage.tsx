import { Grid2, Paper, useMediaQuery } from "@mui/material";
import React, { CSSProperties } from "react";
import { useAuth } from "src/_hooks/useAuth";

import LoginTab from "./_components/LoginTab";
import RegisterTab from "./_components/RegisterTab";
import {
  authPageContainerStyle,
  innerContainerStyleBase,
  pageContainerStyleBase,
} from "src/_lib/_styles/containerStyles";
import {
  formContainerStyleBase,
  formWrapperStyleBase,
} from "src/_lib/_styles/formStyles";

const AuthenticationPage: React.FC = () => {
  const { isAuthenticated, onLogin } = useAuth();
  const isMobile = useMediaQuery("(max-width:600px)");
  const [selectedTab, setSelectedTab] = React.useState<number>(0);
  console.log(isAuthenticated);

  const containerStyle: CSSProperties = {
    ...pageContainerStyleBase,
    ...authPageContainerStyle,
  };

  const innercontainerStyle: CSSProperties = {
    ...innerContainerStyleBase,
    display: "flex",
    justifyItems: "center",
    alignItems: "center",
    padding: "1rem",
    height: "100vh",
  };

  return (
    <Grid2 style={containerStyle}>
      <Grid2 style={innercontainerStyle}>
        <Grid2 style={formContainerStyleBase}>
          <Paper
            style={{ ...formWrapperStyleBase, width: isMobile ? "80%" : "30%" }}
          >
            {selectedTab === 0 && (
              <LoginTab
                tabIndex={0}
                selectedTabIndex={selectedTab}
                onLogin={onLogin}
                onSelectedTabChanged={setSelectedTab}
              />
            )}
            {selectedTab === 1 && (
              <RegisterTab
                tabIndex={1}
                selectedTabIndex={selectedTab}
                onSelectedTabChanged={setSelectedTab}
              />
            )}
          </Paper>
        </Grid2>
      </Grid2>
    </Grid2>
  );
};

export default AuthenticationPage;
