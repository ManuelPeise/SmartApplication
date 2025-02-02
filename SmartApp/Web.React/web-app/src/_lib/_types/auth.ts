export type LoginData = {
  email: string;
  password: string;
};

export type AccountRequest = {
  firstName: string;
  lastName: string;
  email: string;
};

export type JwtTokenData = {
  userId: number;
  name: string;
  email: string;
  userRole: string;
  isActive: boolean;
};

export type AuthenticationState = {
  jwtData: JwtTokenData;
  token: string;
  isAuthenticated: boolean;
};

export type AuthContextProps = {
  isLoading: boolean;
  authenticationState: AuthenticationState | null;
  onLogin: (data: LoginData) => Promise<boolean>;
  onLogout: (userId: number) => Promise<boolean>;
  onRegister: (model: AccountRequest) => Promise<boolean>;
};

export type UserAccessRightModel = {
  userId: number;
  userName?: string;
  accessRights: AccessRight[];
};

export type AccessRight = {
  id: number;
  group: string;
  name: string;
  canView: boolean;
  canEdit: boolean;
  deny: boolean;
};

export type AccessRightValues = Pick<
  AccessRight,
  "canView" | "deny" | "canEdit"
>;

export type AccessRightContextProps = {
  accessRights: UserAccessRightModel | null;
  userHasAssess: (name: string, value: keyof AccessRightValues) => boolean;
  getAccessRight: (name: string) => {
    deny: boolean;
    canView: boolean;
    canEdit: boolean;
  };
};
