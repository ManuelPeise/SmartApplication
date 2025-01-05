import React from "react";
import { useAuth } from "./useAuth";
import { AxiosClient } from "src/_lib/_api/AxiosClient";
import { ApiResponse } from "src/_lib/_types/api";

export type StatefulApiOptions = {
  serviceUrl: string;
  parameters?: { [key: string]: string };
  forceRequest?: boolean;
};

export type StatefulApiResult<T> = {
  isLoading: boolean;
  error: string;
  items: T[];
  get: (options?: Partial<StatefulApiOptions>) => Promise<void>;
};

export const useStatefulApi = <T>(
  options: StatefulApiOptions
): StatefulApiResult<T> => {
  const apiOptionsRef = React.useRef(options);
  const [isLoading, setIsLoading] = React.useState<boolean>(false);
  const [error, setError] = React.useState<string | null>(null);
  const [cache, setCache] = React.useState<{ [key: string]: T[] }>({});

  const { authenticationState } = useAuth();

  const setToken = React.useCallback((token: string) => {
    AxiosClient.defaults.headers.common["Authorization"] = `bearer ${token}`;
  }, []);

  const rejectRequest = React.useCallback((errorMessage: string): void => {
    setError(errorMessage);
  }, []);

  const getRequestUrl = React.useCallback(
    (url: string, params?: { [key: string]: string }) => {
      if (!params) {
        return url;
      }

      const uri = new URL(url);

      Object.keys(params).forEach((key) =>
        uri.searchParams.append(key, params[key])
      );

      return uri.toString();
    },
    []
  );

  const resolveRequest = React.useCallback(
    (response: T | T[], url: string): void => {
      const cacheCopy = { ...cache };
      if (Array.isArray(response)) {
        cacheCopy[url] = response;
      } else {
        cacheCopy[url] = [response];
      }

      setCache(cacheCopy);
    },
    [cache]
  );

  const get = React.useCallback(
    async (partialOptions?: Partial<StatefulApiOptions>) => {
      apiOptionsRef.current = {
        ...options,
        ...partialOptions,
      };

      setToken(authenticationState.token);
      const requestUrl = getRequestUrl(
        apiOptionsRef.current.serviceUrl,
        apiOptionsRef.current.parameters
      );

      if (!apiOptionsRef.current.forceRequest && cache[requestUrl]) {
        return;
      }

      await new Promise(async (resolve, reject) => {
        try {
          setIsLoading(true);
          await AxiosClient.get(requestUrl, {
            headers: { "Content-Type": "application/json" },
          }).then((res) => {
            if (res.status === 200) {
              const response: ApiResponse<T> = res.data;
              resolve(resolveRequest(response.data, requestUrl));
            } else {
              throw new Error(`Request failed with status: ${res.status}`);
            }
          });
        } catch (err) {
          reject(rejectRequest(err.message));
        } finally {
          setIsLoading(false);
        }
      });
    },
    [
      options,
      authenticationState.token,
      cache,
      setToken,
      resolveRequest,
      rejectRequest,
      getRequestUrl,
    ]
  );

  React.useEffect(() => {
    const sendRequest = async () => {
      await get();
    };

    sendRequest();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return {
    isLoading,
    error,
    items: cache[options.serviceUrl],
    get: get,
  };
};
