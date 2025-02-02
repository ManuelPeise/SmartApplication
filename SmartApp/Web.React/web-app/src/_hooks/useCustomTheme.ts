import { useTheme } from "@mui/material";
import { customTheme } from "src/theme";

export const useCustomTheme = () => {
  const theme = useTheme();

  return { theme: { ...theme, ...customTheme } };
};
