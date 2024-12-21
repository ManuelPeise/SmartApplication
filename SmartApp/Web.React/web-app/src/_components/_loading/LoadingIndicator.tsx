import { Box, LinearProgress } from "@mui/material";
import React from "react";
import { colors } from "src/_lib/colors";

interface IProps {
  isLoading: boolean;
}

const LoadingIndicator: React.FC<IProps> = (props) => {
  const { isLoading } = props;
  return (
    <Box sx={{ width: "100%", height: ".1rem" }}>
      {isLoading === true && (
        <LinearProgress
          style={{
            height: "2px",
            backgroundColor: colors.loading,
          }}
          variant="indeterminate"
        />
      )}
    </Box>
  );
};

export default LoadingIndicator;
