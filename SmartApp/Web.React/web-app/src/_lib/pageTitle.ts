export const getAppbarTitle = (location: string) => {
  switch (location) {
    case "/":
      return `${process.env.REACT_APP_Name} - Home`;
    default:
      return "";
  }
};
