import { Button } from "@mui/material";
import React, { CSSProperties } from "react";

interface IProps {
  label: string;
  onAction: () => Promise<void> | void;
  disabled?: boolean;
  cssProperties?: CSSProperties;
}

const FormButton: React.FC<IProps> = (props) => {
  const { label, disabled, cssProperties, onAction } = props;

  return (
    <Button
      style={{
        ...cssProperties,
        padding: ".2rem .5rem",
        color: "#fff",
        backgroundColor: disabled ? "#ffffff" : "#3399ff",
      }}
      disabled={disabled}
      onClick={onAction}
    >
      {label}
    </Button>
  );
};

export default FormButton;
