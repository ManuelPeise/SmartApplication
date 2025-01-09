import React from "react";
import {
  StatelessApiOptions,
  StatelessApiResult,
} from "src/_lib/_api/StatelessApi";

export const useStatefulApiService = <TModel>(
  api: StatelessApiResult,
  options: StatelessApiOptions,
  token: string
) => {
  const optionsRef = React.useRef(options);
  const [data, setData] = React.useState<TModel | null>(null);
  const [isLoading, setIsLoading] = React.useState<boolean>(false);

  const sendGetRequest = React.useCallback(
    async (apiOptions?: StatelessApiOptions) => {
      setIsLoading(true);
      await api
        .get<TModel>(apiOptions ?? optionsRef.current, token)
        .then((res) => {
          setData(res);
        });
      setIsLoading(false);
    },
    [api, token, optionsRef]
  );

  const rebindData = React.useCallback(async () => {
    setIsLoading(true);
    await api.get<TModel>(optionsRef.current, token).then((res) => {
      setData(res);
    });
    setIsLoading(false);
  }, [api, token]);

  const sendPost = React.useCallback(
    async <T>(apiOptions: StatelessApiOptions) => {
      setIsLoading(true);
      const result = await api.post<T>(apiOptions, token);
      setIsLoading(false);
      return result;
    },
    [api, token]
  );

  React.useEffect(() => {
    rebindData();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return {
    data,
    isLoading,
    sendGetRequest,
    rebindData,
    sendPost,
  };
};
