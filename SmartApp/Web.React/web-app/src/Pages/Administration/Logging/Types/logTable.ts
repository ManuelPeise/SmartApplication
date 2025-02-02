export type LogTableColumnDefinition = {
  field: string;
  headerName: string;
  width: number | string;
};

export type LogTableFilter = {
  date: string;
  type: number;
};
