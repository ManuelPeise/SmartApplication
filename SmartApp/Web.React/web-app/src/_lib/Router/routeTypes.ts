export enum UserRightTypeEnum {
  UserAdministration = "UserAdministration",
  MessageLog = "MessageLog",
  EmailAccountSettings = "EmailAccountSettings",
  EmailCleanerSettings = "EmailCleanerSettings",
}

export type RouteProps = {
  requiredRight?: UserRightTypeEnum;
};
