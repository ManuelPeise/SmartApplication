import React from "react";
import { FolderMappingColumnProps } from "../types";
import {
  Checkbox,
  Grid2,
  IconButton,
  MenuItem,
  Select,
  SelectChangeEvent,
  Tooltip,
  Typography,
} from "@mui/material";
import { OnlinePredictionRounded } from "@mui/icons-material";
import { CellMeasurer } from "react-virtualized";

export function checkboxCell(props: FolderMappingColumnProps): React.ReactNode {
  const { id, columnIndex, rowIndex, parent, cache, columnDefinition, data } =
    props;
  return (
    <CellMeasurer
      key={`row-${id}-col-${columnDefinition.dataKey}`}
      columnIndex={columnIndex}
      rowIndex={rowIndex}
      parent={parent}
      cache={cache}
    >
      <Grid2
        key={`row-${id}-col-${columnDefinition.dataKey}`}
        height={columnDefinition.columnHeight}
        sx={{
          display: "flex",
          alignItems: "center",
          alignContent: columnDefinition.align,
          minWidth: columnDefinition.minWidth,
        }}
      >
        <Checkbox
          checked={data[columnDefinition.dataKey] as boolean}
          onChange={(e) =>
            columnDefinition.handleChange(
              { isActive: e.currentTarget.checked },
              id
            )
          }
        />
      </Grid2>
    </CellMeasurer>
  );
}

export function dropdownCell(props: FolderMappingColumnProps): React.ReactNode {
  const { id, columnDefinition, data, targetFolderDropdownItems } = props;

  const handleChange = (e: SelectChangeEvent<number>) => {
    columnDefinition.handleChange(
      { targetFolderId: e.target.value as number },
      data.id
    );
  };

  return (
    <Grid2
      key={`row-${id}-col-${columnDefinition.dataKey}`}
      height={columnDefinition.columnHeight}
      sx={{
        display: "flex",
        alignItems: "center",
        alignContent: columnDefinition.align,
        minWidth: columnDefinition.minWidth,
      }}
    >
      <Select
        disabled={!data.isActive}
        variant="standard"
        disableUnderline
        value={data[columnDefinition.dataKey] as number}
        onChange={handleChange}
        sx={{ minWidth: columnDefinition.minWidth }}
      >
        {targetFolderDropdownItems.map((item) => (
          <MenuItem
            key={id}
            value={item.key}
            disabled={data[columnDefinition.dataKey] === item.key}
          >
            {item.label}
          </MenuItem>
        ))}
      </Select>
    </Grid2>
  );
}

export function folderPredictionCell(
  props: FolderMappingColumnProps
): React.ReactNode {
  const {
    id,
    rowIndex,
    columnIndex,
    parent,
    cache,
    columnDefinition,
    data,
    targetFolderDropdownItems,
  } = props;

  const predictedLabel = targetFolderDropdownItems.find(
    (x) => x.key === data.predictedTargetFolderId
  ).label;

  return (
    <CellMeasurer
      key={`row-${id}-col-${columnDefinition.dataKey}`}
      columnIndex={columnIndex}
      rowIndex={rowIndex}
      parent={parent}
      cache={cache}
    >
      <Grid2
        height={columnDefinition.columnHeight}
        sx={{
          display: "flex",
          alignItems: "center",
          alignContent: columnDefinition.align,
          minWidth: columnDefinition.minWidth,
        }}
      >
        <Grid2
          width="100%"
          display="flex"
          flexDirection="row"
          justifyContent="space-between"
          alignItems="center"
        >
          <Typography>{predictedLabel}</Typography>
          <Tooltip
            title={columnDefinition.toolTipLabel}
            children={
              <IconButton size="small" disabled={!data.isActive}>
                <OnlinePredictionRounded />
              </IconButton>
            }
          />
        </Grid2>
      </Grid2>
    </CellMeasurer>
  );
}

export function labelCell(props: FolderMappingColumnProps): React.ReactNode {
  const { columnDefinition, data } = props;
  return (
    <Grid2
      key={`row-${data.id}-col-${columnDefinition.dataKey}`}
      height={columnDefinition.columnHeight}
      width={columnDefinition.minWidth}
      sx={{
        display: "flex",
        alignItems: "center",
        alignContent: columnDefinition.align,
        minWidth: columnDefinition.minWidth,
      }}
    >
      <Typography>{data[columnDefinition.dataKey]}</Typography>
    </Grid2>
  );
}
