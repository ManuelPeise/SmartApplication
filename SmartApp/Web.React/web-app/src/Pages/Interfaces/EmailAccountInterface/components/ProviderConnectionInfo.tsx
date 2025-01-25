import React from "react";
import { EmailAccountSettings } from "../types";
import { Grid2, Typography } from "@mui/material";
import { useI18n } from "src/_hooks/useI18n";
import { CheckRounded, CloseRounded } from "@mui/icons-material";

interface IProps {
  state: EmailAccountSettings;
}

const ProviderConnectionInfo: React.FC<IProps> = (props) => {
  const { state } = props;
  const { getResource } = useI18n();

  return (
    <Grid2
      container
      size={12}
      spacing={1}
      display="flex"
      width="inherit"
      justifyContent="flex-end"
      alignItems="flex-end"
      paddingTop={4}
      gap={4}
    >
      <Grid2
        size={12}
        display="flex"
        justifyContent="flex-end"
        alignItems="center"
        gap={2}
        pt={2}
      >
        <Grid2 width="49%">
          <Typography>
            {getResource("interface.labelConnectionTestpassed")}
          </Typography>
        </Grid2>
        <Grid2
          width="49%"
          display="flex"
          justifyContent="flex-end"
          paddingRight={2}
        >
          {state.connectionTestPassed ? <CheckRounded /> : <CloseRounded />}
        </Grid2>
      </Grid2>
    </Grid2>
  );
};

export default ProviderConnectionInfo;
