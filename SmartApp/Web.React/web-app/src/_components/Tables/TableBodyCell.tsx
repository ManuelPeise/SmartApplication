import { Box, TableCell, Tooltip, Typography } from "@mui/material";
import React from "react";
import { TableCellBodyProps, TableCellProps } from "./tableTypes";

const typographyTableCellContent = (props: TableCellProps) => {
  const { align, value } = props;

  return (
    <Tooltip
      title={value as string}
      children={
        <Typography
          variant="body2"
          sx={{
            textAlign: align,
            whiteSpace: "nowrap",
            overflow: "hidden",
          }}
        >
          {value}
        </Typography>
      }
    />
  );
};

const TableBodyCell: React.FC<TableCellBodyProps<TableCellProps>> = (props) => {
  const { componentType, componentProps, align, minWidth } = props;

  let childComponent;

  switch (componentType) {
    case "typography":
      childComponent = typographyTableCellContent(componentProps);
      break;
    case "checkbox":
    case "dropdown":
    case "icon":
      childComponent = (
        <Typography
          sx={{
            overflow: "hidden",
            textAlign: "start",
            textOverflow: "ellipsis",
          }}
        >
          {componentProps.value as string}
        </Typography>
      );
  }
  return (
    <TableCell
      sx={{
        width: "100%",
        minWidth: minWidth,
        overflow: "hidden",
        borderRight: "1px solid lightgray",
      }}
      align={align}
    >
      <Box>{childComponent}</Box>
    </TableCell>
  );
};

export default TableBodyCell;
