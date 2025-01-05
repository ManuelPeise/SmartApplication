import { Box, ListItem, Typography } from "@mui/material";
import React, { PropsWithChildren } from "react";

interface IProps extends PropsWithChildren {
  label: string;
}

const ListItemInput: React.FC<IProps> = (props) => {
  const { label, children } = props;
  return (
    <ListItem divider>
      <Box
        display="flex"
        justifyContent="space-between"
        alignItems="flex-end"
        width="100%"
      >
        <Box>
          <Typography>{label}</Typography>
        </Box>
        <Box display="flex" justifyContent="flex-end" minWidth="250px">
          {children}
        </Box>
      </Box>
    </ListItem>
  );
};

export default ListItemInput;
