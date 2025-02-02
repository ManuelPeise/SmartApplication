export type BodyCellComponentType =
  | "typography"
  | "checkbox"
  | "dropdown"
  | "icon";

export type TableColumnDefinition<TModel> = {
  cellKey: number;
  dataKey: keyof TModel;
  headerLabel: string;
  minWidth: number;
  align?: "left" | "center" | "right";
  isCollapible?: boolean;
  isCollapsed?: boolean;
  collapsedWidth?: number;
};

export type TableRowModel<TModel> = {
  rowId: number;
  row: TModel;
};

export type TableCellProps = {
  value: string | boolean | number;
  align?: "left" | "center" | "right";
  onChange?: (checked: boolean, rowIndex: number) => void;
};

export type TableCellBodyProps<TType = TableCellProps> = {
  minWidth: number;
  componentType: BodyCellComponentType;
  componentProps: TType;
  align?: "left" | "center" | "right";
  isCollapsed: boolean;
};
