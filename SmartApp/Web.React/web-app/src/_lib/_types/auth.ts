export type LoginData = {
  email: string;
  password: string;
};

export type AccountRequest = {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  passwordValidation: string;
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
