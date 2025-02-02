import { DropDownItem } from "src/_components/Input/Dropdown";

export type EmailFolderModel = {
  folderId: number;
  resourceKey: string;
};

export type ColumnDefinition<TModel> = {
  name: keyof TModel | "update";
  width?: number;
};

export type EmailClassificationModel = {
  id: number;
  emailAddress: string;
  domain: string;
  subject: string;
  targetFolderId: number;
  predictedTargetFolderId: number | null;
  isSpam: boolean;
  predictedAsSpam: boolean;
  backup: boolean;
  delete: boolean;
};

export type EmailClassificationPageModel = {
  accountId: number;
  spamPredictionEnabled: boolean;
  folderPredictionEnabled: boolean;
  folders: EmailFolderModel[];
  classificationModels: EmailClassificationModel[];
};

export type TableCellProps<TModel> = {
  rowIndex: number;
  colIndex: number;
  model: TModel;
  dataKey: keyof TModel | "update";
  columnDefinition: EmailTableColumn<TModel>;
  disabled?: boolean;
  dropdownItems?: DropDownItem[];
  handleChange?: (partialModel: Partial<TModel>, rowIndex: number) => void;
};

export type EmailTableColumn<TModel> = ColumnDefinition<TModel> & {
  headerLabel: string;
  width: number;
  align: "left" | "center" | "right";
  isReadonly?: boolean;
  hasToolTip: boolean;
  component?: (cellProps: TableCellProps<TModel>) => React.ReactNode;
};

export type EmailFilter = {
  address: string;
  hideHam: boolean;
  hideSpam: boolean;
  modifyEntireFilterResult: boolean;
};
