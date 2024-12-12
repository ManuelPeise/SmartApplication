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
import { ApiResponse, AuthTokenResponse } from "src/_lib/_types/response";

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
    async (data: LoginData) => {
      setIsLoading(true);

      await AxiosClient.post("Authentication/Authenticate", data, {
        headers: { "Content-Type": "application/json" },
      }).then((res) => {
        if (res.status === 200) {
          const tokenResponse: ApiResponse<AuthTokenResponse> = res.data;

          try {
            if (tokenResponse.success) {
              console.log(tokenResponse);
              decodeAndSetToken(tokenResponse.data.token);
            }
          } catch (err) {
            console.log(err);
          }
        }
      });

      setIsLoading(false);
    },
    [decodeAndSetToken]
  );

  const onLogout = React.useCallback(async (userId: number) => {
    setIsLoading(true);

    setIsLoading(false);
  }, []);

  const onRegister = React.useCallback(async (model: AccountRequest) => {}, []);

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
