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
