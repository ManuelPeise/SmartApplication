export enum UserRightTypeEnum {
  UserAdministration = "UserAdministration",
  MessageLog = "MessageLog",
  EmailAccountSettings = "EmailAccountSettings",
}

export type RouteProps = {
  requiredRight?: UserRightTypeEnum;
};
