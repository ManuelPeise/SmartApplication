import { Grid2, Typography } from "@mui/material";
import React from "react";

const SandboxPage: React.FC = () => {
  return (
    <Grid2 container>
      <Grid2 size={12}>
        <Typography>Sandbox</Typography>
      </Grid2>
      <Grid2></Grid2>
    </Grid2>
  );
};

export default SandboxPage;
