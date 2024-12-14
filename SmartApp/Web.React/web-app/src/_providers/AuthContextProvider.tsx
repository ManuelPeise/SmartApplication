import { jwtDecode } from "jwt-decode";
import React, { PropsWithChildren } from "react";
import { AxiosClient } from "src/_lib/_api/AxiosClient";
import {
  AccountRequest,
  AuthContextProps,
  AuthenticationState,
  JwtTokenData,
  LoginData,
} from "src/_lib/_types/auth";
import {
  ApiResponse,
  AuthTokenResponse,
  LogoutResponse,
  SuccessResponse,
} from "src/_lib/_types/response";

export const AuthContext = React.createContext<AuthContextProps>(
  {} as AuthContextProps
);

const AuthContextProvider: React.FC<PropsWithChildren> = (props) => {
  const { children } = props;

  const [authenticationState, setAuthenticationState] =
    React.useState<AuthenticationState | null>(null);
  const [isLoading, setIsLoading] = React.useState<boolean>(false);

  const decodeAndSetToken = React.useCallback((token: string) => {
    const jwt = jwtDecode<JwtTokenData>(token);
    setAuthenticationState({
      jwtData: jwt,
      isAuthenticated: true,
      token: token,
    });
  }, []);

  const onLogin = React.useCallback(
    async (data: LoginData): Promise<boolean> => {
      setIsLoading(true);

      const success = await AxiosClient.post(
        "Authentication/Authenticate",
        data,
        {
          headers: { "Content-Type": "application/json" },
        }
      ).then((res) => {
        if (res.status === 200) {
          const tokenResponse: ApiResponse<AuthTokenResponse> = res.data;

          if (tokenResponse.success) {
            console.log(tokenResponse);
            decodeAndSetToken(tokenResponse.data.token);

            return true;
          }
          return false;
        }

        return false;
      });

      setIsLoading(false);

      return success;
    },
    [decodeAndSetToken]
  );

  const onLogout = React.useCallback(
    async (userId: number): Promise<boolean> => {
      setIsLoading(true);

      const success = await AxiosClient.get(
        `Authentication/Logout?userId=${userId}`,
        {
          headers: { "Content-Type": "application/json" },
        }
      ).then((res) => {
        if (res.status === 200) {
          const logoutResponse: ApiResponse<LogoutResponse> = res.data;

          if (logoutResponse.success && logoutResponse.data.isLoggedOut) {
            setAuthenticationState(null);

            return true;
          }

          return false;
        }

        return false;
      });
      setIsLoading(false);

      return success;
    },
    []
  );

  const onRegister = React.useCallback(
    async (model: AccountRequest): Promise<boolean> => {
      setIsLoading(true);
      const success = await AxiosClient.post("Account/RequestAccount", model, {
        headers: { "Content-Type": "application/json" },
      }).then((res) => {
        if (res.status === 200) {
          const registrationResponse: ApiResponse<SuccessResponse> = res.data;

          if (
            registrationResponse.success &&
            registrationResponse.data.success
          ) {
            return true;
          }

          return false;
        }

        return false;
      });

      setIsLoading(false);

      return success;
    },
    []
  );

  return (
    <AuthContext.Provider
      value={{
        isLoading,
        authenticationState,
        onLogin,
        onLogout,
        onRegister,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export default AuthContextProvider;
