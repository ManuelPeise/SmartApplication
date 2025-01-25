import { Alert, Typography } from "@mui/material";
import React from "react";

interface IProps {
  open: boolean;
  timeOut: number;
  success: boolean;
  text: string;
  handleClose: () => void;
}

const EmailMappingImportAlert: React.FC<IProps> = (props) => {
  const { open, text, success, timeOut, handleClose } = props;

  React.useEffect(() => {
    const timer = setTimeout(() => {
      handleClose();
    }, timeOut);
    return () => clearTimeout(timer);
  }, [handleClose, timeOut, open]);

  if (!open) {
    return null;
  }

  return (
    <Alert
      sx={{
        position: "absolute",
        top: "70px",
        padding: 0.5,
        right: 20,
        minWidth: "10rem",
      }}
      variant="filled"
      severity={success ? "success" : "error"}
    >
      <Typography variant="body2">{text}</Typography>
    </Alert>
  );
};

export default React.memo(EmailMappingImportAlert);
