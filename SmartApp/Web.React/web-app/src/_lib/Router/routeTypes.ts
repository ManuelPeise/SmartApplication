export enum UserRightTypeEnum {
  UserAdministration = "UserAdministration",
  MessageLog = "MessageLog",
  EmailAccountInterface = "EmailAccountInterface",
}

export type RouteProps = {
  requiredRight?: UserRightTypeEnum;
};
