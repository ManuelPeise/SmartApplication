import { AxiosInstance } from "axios";
import { AxiosClient } from "./AxiosClient";

export type StatelessApiOptions = {
  serviceUrl: string;
  parameters?: { [key: string]: string };
  body?: any;
};

export type StatelessApiResult = {
  get: <TModel>(options: StatelessApiOptions, token: string) => Promise<TModel>;
  post: <TModel>(
    options: StatelessApiOptions,
    token: string
  ) => Promise<TModel>;
};

export class StatelessApi {
  static create = (): StatelessApiResult => {
    return {
      get: sendGetRequest,
      post: sendPostRequest,
    } as StatelessApiResult;
  };
}

function setToken(client: AxiosInstance, token: string): void {
  client.defaults.headers.common["Authorization"] = `bearer ${token}`;
}

function getRequestUrl(
  url: string,
  params?: { [key: string]: string }
): string {
  if (!params) {
    return url;
  }

  let requestParameter = "";

  Object.keys(params).forEach(
    (key) => (requestParameter += `${key}=${params[key]}`)
  );

  return `${url}?${requestParameter}`;
}

function sendGetRequest<TModel>(
  options: StatelessApiOptions,
  token: string
): Promise<TModel> {
  return new Promise<TModel>(async (resolve, reject) => {
    try {
      setToken(AxiosClient, token);

      AxiosClient.get(getRequestUrl(options.serviceUrl, options.parameters), {
        headers: { "Content-Type": "application/json" },
      }).then((res) => {
        if (res.status === 200) {
          const responseModel: TModel = res.data;

          return resolve(responseModel);
        } else if (res.status === 204) {
          return resolve(null);
        }
      });
    } catch (err) {
      reject(err.message);
    }
  });
}

function sendPostRequest<TModel>(
  options: StatelessApiOptions,
  token: string
): Promise<TModel> {
  return new Promise<TModel>(async (resolve, reject) => {
    try {
      setToken(AxiosClient, token);
      AxiosClient.post(options.serviceUrl, options.body, {
        headers: { "Content-Type": "application/json" },
      }).then((res) => {
        if (res.status === 200) {
          const responseModel: TModel = res.data;

          return resolve(responseModel);
        } else {
          throw new Error(`Request failed with status: ${res.status}`);
        }
      });
    } catch (err) {
      reject(err.message);
    }
  });
}
