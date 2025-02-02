import {
  NavigateBeforeRounded,
  NavigateNextRounded,
} from "@mui/icons-material";
import { Box, IconButton, TableCell, Typography } from "@mui/material";
import React from "react";
import { CellStyle } from "src/_hooks/useTableDataReducer";

interface IProps {
  cellKey: number;
  headerLabel: string;
  headerStyle: CellStyle;
  isCollapsible?: boolean;
  isCollapsed?: boolean;
  handleToggleCollapsibleCell?: (cellKey: number) => void;
}

const CollapsibleTableHeaderCell: React.FC<IProps> = (props) => {
  const {
    cellKey,
    headerLabel,
    headerStyle,
    isCollapsed,
    handleToggleCollapsibleCell,
  } = props;

  console.log("collapsed?", isCollapsed);

  return (
    <TableCell
      style={{
        display: "flex",
        flexDirection: "row",
        justifyContent: "space-between",
        backgroundColor: cellKey % 2 === 0 ? "gray" : "#fff",
        minWidth: isCollapsed
          ? headerStyle.collapsedWidth
          : headerStyle.maxWidth,
        transform: isCollapsed ? "rotate(90deg)" : "",
      }}
    >
      <Box>
        <Typography variant="h6">{headerLabel}</Typography>
      </Box>
      <Box>
        <IconButton onClick={handleToggleCollapsibleCell.bind(null, cellKey)}>
          {isCollapsed ? <NavigateBeforeRounded /> : <NavigateNextRounded />}
        </IconButton>
      </Box>
    </TableCell>
  );
};

export default CollapsibleTableHeaderCell;
