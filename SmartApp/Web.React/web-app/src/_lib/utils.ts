import moment from "moment";
import { DateFormatEnum } from "./_enums/DateFormatEnum";

export const utils = {
  getFormattedDate: (date: string, format: DateFormatEnum) => {
    return moment(date).format(format);
  },
};

export const groupByKey = <T>(
  array: T[],
  key: keyof T
): Record<string, T[]> => {
  const groupedData = array.reduce((prev, item) => {
    if (!prev[item[key] as string]) {
      prev[item[key] as string] = [];
    }
    prev[item[key] as string].push(item);

    return prev;
  }, {} as Record<string, T[]>);

  return groupedData;
};

export const sortBy = <T>(
  array: T[],
  key: keyof T,
  direction: "asc" | "dsc"
) => {
  return array.sort((a, b) =>
    a[key] < b[key]
      ? direction === "dsc"
        ? -1
        : 1
      : direction === "dsc"
      ? 1
      : -1
  );
};

export const distinctBy = <TModel>(
  array: TModel[],
  selector?: (item: TModel) => any
): TModel[] => {
  const selectedItems = new Set();

  return array.filter((item) => {
    const key = selector ? selector(item) : item;
    if (selectedItems.has(key)) {
      return false;
    }

    selectedItems.add(key);
    return true;
  });
};

export const getCalculatedColumnWidth = <TModel>(
  array: TModel[],
  key: keyof TModel,
  unit: "px" | "rem",
  perCharacter?: number | 3
) => {
  let width: number = 0;

  array.forEach((item) => {
    const keyWidth: number = (item[key] as string)?.length * perCharacter;

    if (keyWidth > width) {
      width = keyWidth;
    }
  });

  return `${width}${unit}`;
};

export const extractValues = <TReturnType, TModel>(
  models: TModel[],
  key: keyof TModel
): TReturnType[] => {
  const array: TReturnType[] = [];

  models.forEach((item) => {
    if (!array.includes(item[key] as TReturnType)) {
      array.push(item[key] as TReturnType);
    }
  });

  return array;
};
