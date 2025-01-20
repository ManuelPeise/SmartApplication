import { GridColDef } from "@mui/x-data-grid";

export type GenericDataGridColDef<TModel> = GridColDef<TModel> & {
  field: keyof TModel;
};
