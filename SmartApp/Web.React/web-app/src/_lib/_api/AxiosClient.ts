import axios from "axios";

export const AxiosClient = axios.create({
  baseURL: process.env.REACT_APP_API_BASE_URL,
});
