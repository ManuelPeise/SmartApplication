import { Grid2 } from "@mui/material";
import React, { PropsWithChildren } from "react";
import {
  innerContainerStyleBase,
  pageContainerStyleBase,
} from "src/_lib/_styles/containerStyles";

const PageContainer: React.FC<PropsWithChildren> = (props) => {
  const { children } = props;

  return (
    <Grid2 style={pageContainerStyleBase}>
      <Grid2 style={innerContainerStyleBase}>{children}</Grid2>
    </Grid2>
  );
};

export default PageContainer;
