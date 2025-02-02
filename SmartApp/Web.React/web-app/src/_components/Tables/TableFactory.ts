import { KeysOfTypeBool, KeysOfTypeString } from "src/_lib/CustomTypes";

type TableConfigurationType = "Label" | "Text" | "Checkbox";

type TableCellProps = {
  type: TableConfigurationType;
  maxWidth: number;
  padding?: number;
  bg?: string;
  collapsedWith?: number;
};

type TableCellDataConfiguration<TModel> = {
  dataKey: keyof TModel;
  originalData: TModel[keyof TModel];
  model: TModel;
};

type TableCellConfigurationProps<TModel> = TableCellDataConfiguration<TModel> &
  TableCellProps;

type TableCellPropsInitializationCallback<TModel> = (
  type: TableConfigurationType,
  propertyName: keyof TModel,
  model: TModel,
  maxWith: number,
  padding?: number,
  bg?: string,
  collapsedWith?: number
) => TableCellConfigurationProps<TModel>;

export type TabelConfigurationBase<TModel> = {
  propertyName: keyof TModel;
  type: TableConfigurationType;
  maxWith: number;
  padding?: number;
  bg?: string;
  collapsedWith?: number;
  initializationCallback: TableCellPropsInitializationCallback<TModel>;
};

class TableFactory {
  static createLabelSettings = <TModel>(
    propertyName: KeysOfTypeString<TModel>,
    maxWith: number,
    padding?: number,
    bg?: string,
    collapsedWith?: number
  ): TabelConfigurationBase<TModel> => {
    return {
      propertyName: propertyName,
      type: "Label",
      maxWith: maxWith,
      padding: padding,
      bg: bg,
      collapsedWith: collapsedWith,
      initializationCallback: this.initializeLabelCellProps,
    };
  };

  static createTextSettings = <TModel>(
    propertyName: KeysOfTypeString<TModel>,
    maxWith: number,
    padding?: number,
    bg?: string,
    collapsedWith?: number
  ): TabelConfigurationBase<TModel> => {
    return {
      propertyName: propertyName,
      type: "Text",
      maxWith: maxWith,
      padding: padding,
      bg: bg,
      collapsedWith: collapsedWith,
      initializationCallback: this.initializeTextCellProps,
    };
  };

  static createCheckboxSettings = <TModel>(
    propertyName: KeysOfTypeBool<TModel>,
    maxWith: number,
    padding?: number,
    bg?: string,
    collapsedWith?: number
  ): TabelConfigurationBase<TModel> => {
    return {
      propertyName: propertyName,
      type: "Checkbox",
      maxWith: maxWith,
      padding: padding,
      bg: bg,
      collapsedWith: collapsedWith,
      initializationCallback: this.initializeBooleanCellProps,
    };
  };

  static initializeLabelCellProps = <TModel>(
    type: TableConfigurationType,
    propertyName: keyof TModel,
    model: TModel,
    maxWith: number,
    padding?: number,
    bg?: string,
    collapsedWith?: number
  ): TableCellConfigurationProps<TModel> => {
    return {
      type: type,
      dataKey: propertyName,
      originalData: model[propertyName],
      model: model,
      maxWidth: maxWith,
      padding: padding,
      bg: bg,
      collapsedWith: collapsedWith,
    };
  };

  static initializeTextCellProps = <TModel>(
    type: TableConfigurationType,
    propertyName: keyof TModel,
    model: TModel,
    maxWith: number,
    padding?: number,
    bg?: string,
    collapsedWith?: number
  ): TableCellConfigurationProps<TModel> => {
    return {
      type: type,
      dataKey: propertyName,
      originalData: model[propertyName],
      model: model,
      maxWidth: maxWith,
      padding: padding,
      bg: bg,
      collapsedWith: collapsedWith,
    };
  };

  static initializeBooleanCellProps = <TModel>(
    type: TableConfigurationType,
    propertyName: keyof TModel,
    model: TModel,
    maxWith: number,
    padding?: number,
    bg?: string,
    collapsedWith?: number
  ): TableCellConfigurationProps<TModel> => {
    return {
      type: type,
      dataKey: propertyName,
      originalData: model[propertyName],
      model: model,
      maxWidth: maxWith,
      padding: padding,
      bg: bg,
      collapsedWith: collapsedWith,
    };
  };
}

export default TableFactory;
