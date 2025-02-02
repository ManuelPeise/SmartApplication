import {
  Box,
  Checkbox,
  IconButton,
  MenuItem,
  Select,
  SelectChangeEvent,
  Tooltip,
  Typography,
} from "@mui/material";
import { EmailClassificationModel, TableCellProps } from "../types";
import { OnlinePrediction, UpdateRounded } from "@mui/icons-material";

function labelCell(props: TableCellProps<EmailClassificationModel>) {
  const { rowIndex, colIndex, model, columnDefinition } = props;
  return (
    <Box
      key={`${rowIndex}-${colIndex}-${columnDefinition.name}`}
      sx={{
        width: columnDefinition.width,
        height: "100%",
        padding: 0,
        textOverflow: "ellipsis",
        "&:hover": {
          cursor: columnDefinition.hasToolTip ? "pointer" : "default",
        },
      }}
    >
      {columnDefinition.hasToolTip ? (
        <Tooltip
          title={model[columnDefinition.name]}
          children={<Typography>{model[columnDefinition.name]}</Typography>}
        />
      ) : (
        <Typography sx={{ fontSize: "16px" }}>
          {model[columnDefinition.name]}
        </Typography>
      )}
    </Box>
  );
}

function folderPredictionCell(props: TableCellProps<EmailClassificationModel>) {
  const {
    rowIndex,
    colIndex,
    dataKey,
    dropdownItems,
    model,
    columnDefinition,
  } = props;

  const folderKey = model[dataKey] ?? 1;
  const label = dropdownItems.find((x) => x.key === folderKey).label;
  return (
    <Box
      key={`${rowIndex}-${colIndex}-${columnDefinition.name}`}
      sx={{
        width: columnDefinition.width,
        height: "100%",
        padding: 0,
        textOverflow: "ellipsis",
        display: "flex",
        justifyContent: columnDefinition.align,
        alignItems: columnDefinition.align,
      }}
    >
      <Tooltip title={label} children={<OnlinePrediction />} />
    </Box>
  );
}

function predictionUpdateCell(props: TableCellProps<EmailClassificationModel>) {
  const {
    rowIndex,
    colIndex,

    columnDefinition,
  } = props;

  return (
    <Box
      key={`${rowIndex}-${colIndex}-${columnDefinition.name}`}
      sx={{
        width: columnDefinition.width,
        height: "100%",
        padding: 0,
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
      }}
    >
      <Tooltip
        title={columnDefinition.headerLabel}
        children={
          <IconButton size="small" disabled={columnDefinition.isReadonly}>
            <UpdateRounded />
          </IconButton>
        }
      />
    </Box>
  );
}

function checkboxCell(props: TableCellProps<EmailClassificationModel>) {
  const {
    rowIndex,
    colIndex,
    model,
    dataKey,
    disabled,
    columnDefinition,
    handleChange,
  } = props;

  return (
    <Box
      key={`${rowIndex}-${colIndex}-${columnDefinition.name}`}
      sx={{
        display: "flex",
        width: columnDefinition.width,
        height: "100%",
        padding: 0,
        justifyContent: columnDefinition.align,
        textOverflow: "ellipsis",
        "&:hover": {
          cursor: columnDefinition.hasToolTip ? "pointer" : "default",
        },
      }}
    >
      <Checkbox
        disabled={disabled}
        checked={model[columnDefinition.name] as boolean}
        onChange={(e) =>
          handleChange({ [dataKey]: e.currentTarget.checked }, model.id)
        }
      />
    </Box>
  );
}

function dropdownCell(
  props: TableCellProps<EmailClassificationModel>
): React.ReactNode {
  const {
    rowIndex,
    colIndex,
    model,
    dataKey,
    dropdownItems,
    disabled,
    columnDefinition,
    handleChange,
  } = props;

  const onChange = (e: SelectChangeEvent<number>) => {
    handleChange &&
      handleChange({ [dataKey]: e.target.value as number }, model.id);
  };

  return (
    <Box
      key={`${rowIndex}-${colIndex}-${columnDefinition.name}`}
      sx={{
        width: columnDefinition.width,
        height: "100%",
        padding: 0,
      }}
    >
      <Select
        disabled={disabled || !model.backup}
        variant="standard"
        disableUnderline
        value={model[dataKey] as number}
        onChange={onChange}
        sx={{ minWidth: "100%" }}
      >
        {dropdownItems.map((item) => (
          <MenuItem
            key={item.key}
            value={item.key}
            disabled={model[dataKey] === item.key}
          >
            {item.label}
          </MenuItem>
        ))}
      </Select>
    </Box>
  );
}

export function emailTableCell(
  props: TableCellProps<EmailClassificationModel>
) {
  switch (props.dataKey) {
    case "emailAddress":
    case "domain":
    case "subject":
      return labelCell(props);
    case "isSpam":
    case "predictedAsSpam":
    case "backup":
    case "delete":
      return checkboxCell(props);
    case "targetFolderId":
      return dropdownCell(props);
    case "predictedTargetFolderId":
      return folderPredictionCell(props);
    case "update":
      return predictionUpdateCell(props);
    default:
      return null;
  }
}
