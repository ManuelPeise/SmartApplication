import moment from "moment";
import { DateFormatEnum } from "./_enums/DateFormatEnum";

export const utils = {
  getFormattedDate: (date: string, format: DateFormatEnum) => {
    return moment(date).format(format);
  },
};
