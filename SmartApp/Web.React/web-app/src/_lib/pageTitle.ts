import { routes } from "./AppRouter";

export const getAppbarTitle = (location: string) => {
  switch (location) {
    case routes.home:
      return `${process.env.REACT_APP_Name} - Home`;
    case routes.log:
      return `${process.env.REACT_APP_Name} - Log`;
    default:
      return "";
  }
};
