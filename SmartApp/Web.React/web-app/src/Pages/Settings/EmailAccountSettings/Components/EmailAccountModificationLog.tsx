import { Box, ListItem, Typography } from "@mui/material";
import React from "react";

interface IProps {
  caption: string;
  logMessage: string;
}

const EmailAccountModificationLog: React.FC<IProps> = (props) => {
  const { caption, logMessage } = props;

  return (
    <ListItem>
      <Box
        width="100%"
        display="flex"
        flexDirection="column"
        gap={4}
        marginTop={4}
      >
        <Box width="100%">
          <Typography variant="h5">{caption}</Typography>
        </Box>
        <Box width="100%">
          <Typography variant="body2">{logMessage}</Typography>
        </Box>
      </Box>
    </ListItem>
  );
};

export default EmailAccountModificationLog;
