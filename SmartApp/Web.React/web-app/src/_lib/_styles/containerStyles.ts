import { CSSProperties } from "react";
import authBackground from "../_img/pier.jpg";

export const pageContainerStyleBase: CSSProperties = {
  width: "100Vw",
  minHeight: "100vh",
  maxHeight: "100vh",
  display: "flex",
};

export const authPageContainerStyle: CSSProperties = {
  backgroundImage: `url(${authBackground})`,
  backgroundPosition: "center",
  backgroundRepeat: "no-repeat",
};

export const innerContainerStyleBase: CSSProperties = {
  height: "100%",
  width: "100%",
  overflowX: "scroll",
};
