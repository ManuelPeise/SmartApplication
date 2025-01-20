import { Box, Tooltip, Typography } from "@mui/material";
import React from "react";

interface IProps {
  padding: number;
  value: string;
  toolTipText?: string;
}

const DataGridLabelCell: React.FC<IProps> = (props) => {
  const { padding, value, toolTipText } = props;
  return (
    <Box
      sx={{
        width: "100%",
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        padding: padding,
        "&:hover": {
          cursor: "pointer",
        },
      }}
    >
      <Tooltip
        sx={{
          width: "100%",
          height: "100%",
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
        }}
        title={toolTipText}
        children={
          <Typography
            variant="body1"
            sx={{
              fontSize: ".8rem",
              fontWeight: "400",
              width: "100%",
              height: "100%",
            }}
          >
            {value}
          </Typography>
        }
      />
    </Box>
  );
};

export default DataGridLabelCell;
