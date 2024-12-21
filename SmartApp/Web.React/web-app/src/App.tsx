import React from "react";
import "./_lib/_translation/i18n";
import "./App.css";
import RootLayout from "./_components/_containers/RootLayout";
import AppRouter from "./_lib/AppRouter";
import AuthContextProvider from "./_providers/AuthContextProvider";
import { ThemeProvider, useTheme } from "@mui/material";
import { customTheme } from "./theme";

const App: React.FC = () => {
  const theme = useTheme();
  return (
    <ThemeProvider theme={{ ...theme, ...customTheme }}>
      <RootLayout>
        <AuthContextProvider>
          <AppRouter />
        </AuthContextProvider>
      </RootLayout>
    </ThemeProvider>
  );
};

export default App;
