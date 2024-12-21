export type ApiOptions = {
  isPrivate: boolean;
  data?: string;
  requestUrl: string;
  parameters: string | null;
  initialLoad: boolean;
};

export type ApiResult<T> = {
  isLoading: boolean;
  data: T[] | null;
  requestError?: string | null;
  sendGetRequest: (options?: Partial<ApiOptions>) => Promise<void>;
  sendPostRequest: (options?: Partial<ApiOptions>) => Promise<void>;
  sendPost: <TResponse>(options?: Partial<ApiOptions>) => Promise<TResponse>;
};
