export type ApiResponse<T> = {
  success: boolean;
  data: T;
};

export type SuccessResponse = {
  success: boolean;
};

export type AuthTokenResponse = {
  token: string;
};

export type LogoutResponse = {
  isLoggedOut: boolean;
};
