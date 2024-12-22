import { Box } from "@mui/material";
import React, { PropsWithChildren } from "react";

interface IProps extends PropsWithChildren {}

const FilterLayout: React.FC<IProps> = (props) => {
  const { children } = props;
  return (
    <Box
      display="flex"
      padding={1}
      width="100%"
      height="fit-content"
      flexDirection={{
        xs: "column",
        sm: "column",
        md: "column",
        lg: "column",
        xl: "column",
      }}
      justifyContent="flex-start"
      alignItems="center"
      gap={3}
    >
      <Box width="100%" height="10vh">
        {children[0]}
      </Box>
      <Box width="100%" height="75vh">
        {children[1]}
      </Box>
    </Box>
  );
};

export default FilterLayout;
