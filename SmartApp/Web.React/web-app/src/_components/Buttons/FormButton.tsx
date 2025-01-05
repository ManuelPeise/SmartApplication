import { Button } from "@mui/material";
import React, { CSSProperties } from "react";
import { colors } from "src/_lib/colors";

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
        border: `1px solid ${colors.typography.darkgray}`,
        color: colors.typography.darkgray,
        opacity: disabled ? 0.2 : 1,
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
