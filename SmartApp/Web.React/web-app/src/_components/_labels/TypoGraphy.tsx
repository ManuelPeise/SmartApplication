import { Typography } from "@mui/material";
import React, { CSSProperties } from "react";
import { ResponsiveSize } from "src/types";

interface IProps {
  label: string;
  fontSize?: ResponsiveSize;
  color?: string;
  style?: CSSProperties;
}

const TypoGraphy: React.FC<IProps> = (props) => {
  const { label, fontSize, color, style } = props;
  return (
    <Typography
      sx={{
        fontSize: fontSize,
        color: color,
        ...style,
      }}
    >
      {label}
    </Typography>
  );
};

export default TypoGraphy;
