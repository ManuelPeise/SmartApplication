import { AccessRight } from "src/_lib/_types/auth";

export type UserAdministrationUserModel = {
  userId: number;
  firstName: string;
  lastName: string;
  email: string;
  isActive: boolean;
  isAdmin: boolean;
  accessRights: AccessRight[];
};
