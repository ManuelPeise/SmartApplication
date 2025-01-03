import { UserRoleEnum } from "../_enums/UserRoleEnum";

export type RouteProps = {
  requiredRole: UserRoleEnum;
  redirectUri: string;
};
