export enum UserRightTypeEnum {
  UserAdministration = "UserAdministration",
  MessageLog = "MessageLog",
  EmailAccountInterface = "EmailAccountInterface",
  EmailAccountSettings = "EmailAccountSettings",
  EmailCleanerSettings = "EmailCleanerSettings",
}

export type RouteProps = {
  requiredRight?: UserRightTypeEnum;
};
