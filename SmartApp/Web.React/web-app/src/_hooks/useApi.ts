import React from "react";
import { ApiOptions, ApiResult } from "src/_lib/_types/api";
import { useAuth } from "./useAuth";
import { AxiosClient } from "src/_lib/_api/AxiosClient";

export const useApi = <T>(initializationOptions: ApiOptions): ApiResult<T> => {
  const { authenticationState } = useAuth();
  const [apiOptions, setApiOptions] = React.useState<ApiOptions>(
    initializationOptions
  );
  const [error, setError] = React.useState<string | null>(null);
  const [isLoading, setIsloading] = React.useState<boolean>(false);
  const [data, setData] = React.useState<T>({} as T);

  const get = React.useCallback(
    async (options?: ApiOptions): Promise<void> => {
      if (options) {
        setApiOptions({ ...apiOptions, ...options });
      }

      if (apiOptions.isPrivate) {
        AxiosClient.defaults.headers.common[
          "Authorization"
        ] = `bearer ${authenticationState?.token}`;
      }

      setIsloading(true);

      try {
        await AxiosClient.get(apiOptions.requestUrl, {
          headers: { "Content-Type": "application/json" },
        }).then(async (res) => {
          if (res.status === 200) {
            const responseData: T = res.data;

            console.log("Response:", responseData);
            setData(responseData);
          }
        });
      } catch (err) {
        setError(err.message);
      } finally {
        setIsloading(false);
      }
    },
    [authenticationState, apiOptions]
  );

  const post = React.useCallback(
    async (options?: Partial<ApiOptions>): Promise<boolean> => {
      let success = false;

      if (options.isPrivate) {
        AxiosClient.defaults.headers.common[
          "Authorization"
        ] = `bearer ${authenticationState.token}`;
      }

      try {
        await AxiosClient.post(options.requestUrl, options.data, {
          headers: { "Content-Type": "application/json" },
        }).then(async (res) => {
          if (res.status === 200) {
            success = true;
          }
        });
      } catch (err) {
        setError(err.message);
      } finally {
        setIsloading(false);
      }

      return success;
    },
    [authenticationState]
  );

  React.useEffect(() => {
    const sendInitialRequest = async () => {
      await get();
    };

    if (apiOptions.initialLoad === true) {
      sendInitialRequest();
    }
  }, [apiOptions, get]);

  return {
    isLoading,
    data,
    requestError: error,
    sendGetRequest: get,
    sendPostRequest: post,
  };
};
