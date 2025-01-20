import { Box, TextField } from "@mui/material";
import React from "react";

interface IProps {
  rowId: number;
  padding: number;
  value: string;
  label: string;
  disabled: boolean;
  handleChange: (modelId: number, value: string) => void;
}

const DataGridTextFieldCell: React.FC<IProps> = (props) => {
  const { rowId, padding, value, label, disabled, handleChange } = props;
  return (
    <Box
      sx={{
        width: "100%",
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        padding: padding,
        paddingBottom: 2,
        "&:hover": {
          cursor: "pointer",
        },
      }}
    >
      <TextField
        label={label}
        type="text"
        disabled={disabled}
        variant="standard"
        value={value ?? ""}
        slotProps={{
          input: {
            style: { marginBottom: padding },
          },
        }}
        onChange={(e) => handleChange(rowId, e.currentTarget.value)}
      />
    </Box>
  );
};

export default DataGridTextFieldCell;
