import { Box } from "@mui/material";
import { GenericDataGridColDef } from "./dataGrid";
import { DataGrid } from "@mui/x-data-grid";

interface IProps<TModel> {
  rowHeight?: number;
  maxHeight: number;
  models: TModel[];
  disableFocus?: boolean;
  showHeaderSeperators?: boolean;
  columnDefinitions: GenericDataGridColDef<TModel>[];
}

function GenericDataGrid<TModel>(props: IProps<TModel>) {
  const {
    rowHeight,
    maxHeight,
    models,
    disableFocus,
    showHeaderSeperators,
    columnDefinitions,
  } = props;

  return (
    <Box component="div" sx={{ height: maxHeight, width: "100%" }}>
      <DataGrid
        getRowHeight={() => rowHeight}
        rows={models}
        columns={columnDefinitions}
        disableRowSelectionOnClick
        disableColumnSelector
        disableColumnMenu
        scrollbarSize={0}
        sx={{
          "& .MuiDataGrid-cell": {
            display: "flex",
            alignItems: "center",
          },
          "& .MuiDataGrid-cell:focus": {
            outline: disableFocus ? "none" : "",
          },
          "& .MuiDataGrid-cell:focus-within": {
            outline: disableFocus ? "none" : "",
          },
          "& .MuiDataGrid-columnHeader:focus": {
            outline: disableFocus ? "none" : "",
          },
          "& .MuiDataGrid-columnSeparator": {
            display: !showHeaderSeperators ? "none" : "",
          },
          "& .MuiDataGrid-virtualScroller::-webkit-scrollbar": {
            display: "none",
          },
        }}
      />
    </Box>
  );
}

export default GenericDataGrid;
