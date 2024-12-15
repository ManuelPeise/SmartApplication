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
        border: "1px solid lightgray",
        color: "lightgray",
        opacity: disabled ? 0.5 : 1,
        backgroundColor: "transparent",
      }}
      disabled={disabled}
      onClick={onAction}
    >
      {label}
    </Button>
  );
};

export default FormButton;
