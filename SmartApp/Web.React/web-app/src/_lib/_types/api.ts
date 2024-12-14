export type ApiOptions = {
  isPrivate: boolean;
  data?: string;
  requestUrl: string;
  parameters: string | null;
};

export type ApiResult<T> = {
  isLoading: boolean;
  data: T | T[];
  requestError?: string | null;
  sendGetRequest: (options?: Partial<ApiOptions>) => Promise<void>;
  sendPostRequest: (options?: Partial<ApiOptions>) => Promise<boolean>;
};
