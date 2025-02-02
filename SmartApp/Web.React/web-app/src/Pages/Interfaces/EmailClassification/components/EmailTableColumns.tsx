import {
  Box,
  Checkbox,
  MenuItem,
  Select,
  SelectChangeEvent,
  Tooltip,
  Typography,
} from "@mui/material";
import { EmailClassificationModel, TableCellProps } from "../types";

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

function predictedFolderCell(props: TableCellProps<EmailClassificationModel>) {
  const {
    rowIndex,
    colIndex,
    model,
    dataKey,
    columnDefinition,
    dropdownItems,
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
      }}
    >
      <Typography sx={{ opacity: 0.7 }}>{label}</Typography>
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
        disabled={disabled}
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
      return checkboxCell(props);
    case "targetFolderId":
      return dropdownCell(props);
    case "predictedTargetFolderId":
      return predictedFolderCell(props);
    default:
      return null;
  }
}
