import { createTheme } from "@mui/material";

export const customTheme = createTheme({
  palette: {
    primary: {
      main: "#0000ff",
      light: "#4d4dff",
      dark: "#00004d",
    },
    secondary: {
      main: "#f2f2f2",
      light: "#f2f2f2",
      dark: "#bfbfbf",
    },
  },
  breakpoints: {
    values: {
      xs: 0,
      sm: 600,
      md: 900,
      lg: 1200,
      xl: 1900,
    },
  },
});
