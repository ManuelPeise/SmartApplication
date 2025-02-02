import { Box, TableCell, Typography } from "@mui/material";
import React from "react";

interface IProps {
  cellKey: number;
  minWidth: number;
  headerLabel: string;
  align?: "left" | "center" | "right";
}

const HeaderCell: React.FC<IProps> = (props) => {
  const { cellKey, headerLabel, minWidth, align } = props;

  return (
    <TableCell
      id={`header-cell-${cellKey}`}
      align={align ?? "center"}
      sx={{
        width: "100%",
        minWidth: minWidth,
        backgroundColor: "#fff",
      }}
    >
      <Box sx={{ width: "100%", padding: "1px" }}>
        <Typography sx={{ width: "100%", fontWeight: "600" }}>
          {headerLabel}
        </Typography>
      </Box>
    </TableCell>
  );
};

export default HeaderCell;
