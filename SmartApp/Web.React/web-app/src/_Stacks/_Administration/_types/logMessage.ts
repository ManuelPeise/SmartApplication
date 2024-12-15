export enum LogmessageTypeEnum {
  info = 0,
  error = 1,
  criticalError = 2,
}

export type LogMessage = {
  id: number;
  timeStamp: string;
  message: string;
  exceptionMessage: string;
  messageType: LogmessageTypeEnum;
};
