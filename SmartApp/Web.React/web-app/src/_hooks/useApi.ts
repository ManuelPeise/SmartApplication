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

  const [data, setData] = React.useState<T[] | null>(null);

  const getRequestUrl = React.useCallback((options: ApiOptions) => {
    const uri = `${options.requestUrl}${
      options.parameters ? options.parameters : ""
    }`;

    console.log("Url:", uri);

    return uri;
  }, []);

  const get = React.useCallback(
    async (options?: Partial<ApiOptions>): Promise<void> => {
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
        await AxiosClient.get(getRequestUrl({ ...apiOptions, ...options }), {
          headers: { "Content-Type": "application/json" },
        }).then(async (res) => {
          if (res.status === 200) {
            if (Array.isArray(res.data)) {
              const data: T[] = JSON.parse(JSON.stringify(res.data));

              setData(data);
            } else {
              setData([res.data]);
            }
          }
        });
      } catch (err) {
        setError(err.message);
      } finally {
        setIsloading(false);
      }
    },
    [authenticationState, apiOptions, getRequestUrl]
  );

  const post = React.useCallback(
    async (options?: Partial<ApiOptions>): Promise<void> => {
      if (options.isPrivate) {
        AxiosClient.defaults.headers.common[
          "Authorization"
        ] = `bearer ${authenticationState.token}`;
      }
      setIsloading(true);
      try {
        await AxiosClient.post(options.requestUrl, options.data, {
          headers: { "Content-Type": "application/json" },
        });
      } catch (err) {
        setError(err.message);
      } finally {
        setIsloading(false);
      }
    },
    [authenticationState]
  );

  const PostWithResponse = React.useCallback(
    async <TResponse>(options?: Partial<ApiOptions>): Promise<TResponse> => {
      let response: TResponse = null;

      if (options.isPrivate) {
        AxiosClient.defaults.headers.common[
          "Authorization"
        ] = `bearer ${authenticationState.token}`;
      }
      setIsloading(true);
      try {
        await AxiosClient.post(options.requestUrl, options.data, {
          headers: { "Content-Type": "application/json" },
        }).then(async (res) => {
          if (res.status === 200) {
            if (res.data) {
              const responseData: TResponse = res.data;

              response = responseData;
            }
          }
        });
      } catch (err) {
        setError(err.message);
      } finally {
        setIsloading(false);
      }

      return response;
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
    sendPost: PostWithResponse,
  };
};
