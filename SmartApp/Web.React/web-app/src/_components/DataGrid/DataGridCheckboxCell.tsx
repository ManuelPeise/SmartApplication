import { Box, Checkbox, Tooltip } from "@mui/material";

type DataGridCheckboxProps = {
  rowId: number;
  padding: number;
  checked: boolean;
  toolTipText?: string;
  disabled: boolean;
  handleCheckedChanged: (rowId: number, checked: boolean) => void;
};

const DataGridCheckboxCell: React.FC<DataGridCheckboxProps> = (props) => {
  const {
    rowId,
    padding,
    checked,
    toolTipText,
    disabled,
    handleCheckedChanged,
  } = props;

  return (
    <Box
      sx={{
        width: "100%",
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        padding: padding,
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
          <Checkbox
            sx={{ maxWidth: 50, maxHeight: 50 }}
            size="small"
            checked={checked}
            disabled={disabled}
            onChange={(e) =>
              handleCheckedChanged(rowId, e.currentTarget.checked)
            }
          />
        }
      />
    </Box>
  );
};

export default DataGridCheckboxCell;
