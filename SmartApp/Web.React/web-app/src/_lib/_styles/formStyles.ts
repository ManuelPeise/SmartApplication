import { CSSProperties } from "react";

export const formContainerStyleBase: CSSProperties = {
  display: "flex",
  justifyContent: "center",
  alignItems: "center",
  height: "100%",
  width: "100%",
};

export const formWrapperStyleBase: CSSProperties = {
  padding: "2rem 2rem",
  width: "80%",
  height: "50%",
  backgroundColor: "#fff",
  borderRadius: "8px",
  opacity: 0.5,
};

export const loginForm: CSSProperties = {
  padding: "1rem",
  width: "100%",
  display: "flex",
  flexDirection: "column",
};

export const formRowBase: CSSProperties = {
  width: "100%",
  padding: ".8rem 0.5rem",
  display: "flex",
  justifyContent: "center",
};

export const iconFormRowBase: CSSProperties = {
  width: "100%",
  display: "flex",
  justifyContent: "center",
  alignItems: "center",
};

export const formHeaderIconBase: CSSProperties = {
  opacity: 0.6,
  color: "#3399ff",
};

export const formTitleStyleBase: CSSProperties = {
  fontSize: "2rem",
  fontStyle: "italic",
  color: "#3399ff",
};

export const formButtonStyleBase: CSSProperties = {
  padding: ".2rem .5rem",
  backgroundColor: "#3399ff",
  color: "#fff",
};
