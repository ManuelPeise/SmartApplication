import { UserAdministrationUserModel } from "./UserAdministrationUserModel";

export type UserAdministrationPageProps = {
  users: UserAdministrationUserModel[];
  onUpdateUser: (model: UserAdministrationUserModel) => Promise<boolean>;
};
