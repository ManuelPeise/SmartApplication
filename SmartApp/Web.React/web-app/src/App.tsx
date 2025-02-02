import React from "react";
import "./_lib/_translation/i18n";
import "./App.css";
import { ThemeProvider, useTheme } from "@mui/material";
import { customTheme } from "./theme";
import AppRouter from "./_lib/Router/AppRouter";

const App: React.FC = () => {
  const theme = useTheme();
  return (
    <ThemeProvider theme={{ ...theme, ...customTheme }}>
      <AppRouter />
    </ThemeProvider>
  );
};

export default App;
