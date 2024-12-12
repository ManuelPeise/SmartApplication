import { Button } from "@mui/material";
import React, { CSSProperties } from "react";
import { formButtonStyleBase } from "src/_lib/_styles/formStyles";

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
      style={{ ...formButtonStyleBase, ...cssProperties }}
      disabled={disabled}
      onClick={onAction}
    >
      {label}
    </Button>
  );
};

export default FormButton;
