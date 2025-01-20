import { Backdrop } from "@mui/material";
import React, { PropsWithChildren } from "react";

interface IProps extends PropsWithChildren {
  open: boolean;
}

const EmailCleanerOverlay: React.FC<IProps> = (props) => {
  const { children, open } = props;

  return (
    <Backdrop sx={{ zIndex: 1000 }} open={open}>
      {children}
    </Backdrop>
  );
};

export default EmailCleanerOverlay;
