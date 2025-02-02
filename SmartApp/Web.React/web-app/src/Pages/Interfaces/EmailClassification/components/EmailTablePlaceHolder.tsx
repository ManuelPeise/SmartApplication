import { Box, Typography } from "@mui/material";
import React from "react";
import { useI18n } from "src/_hooks/useI18n";

const EmailTablePlaceHolder: React.FC = () => {
  const { getResource } = useI18n();
  return (
    <Box display="flex" justifyContent="center" width="100%">
      <Box
        display="flex"
        justifyContent="center"
        width="50%"
        border="1px solid red"
        borderRadius="8px"
        marginTop={4}
        padding={2}
      >
        <Typography variant="body1" sx={{ fontSize: "16px", fontWeight: 600 }}>
          {getResource("interface.placeholderTextNoContent")}
        </Typography>
      </Box>
    </Box>
  );
};

export default EmailTablePlaceHolder;
