import { Routes } from "src/types";

export const getRoutes = (): Routes => {
  return {
    administration: "/administration",
    log: "/administration/log",
    home: "/",
    private: "/private",
  };
};

export const browserRoutes = {
  home: "/",
  login: "/login",
  register: "/register",
  log: "/administration/log",
};
