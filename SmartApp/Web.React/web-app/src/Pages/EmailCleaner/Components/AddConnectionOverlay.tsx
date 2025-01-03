import { Box } from "@mui/material";
import React, { PropsWithChildren } from "react";

interface IProps extends PropsWithChildren {
  handleHideConnectionScreen: () => void;
}

const AddConnectionOverlay: React.FC<IProps> = (props) => {
  const { children, handleHideConnectionScreen } = props;
  return (
    <Box
      sx={{
        position: "absolute",
        top: 0,
        left: 0,
        height: "100%",
        width: "100%",
        backgroundColor: "#000",
        opacity: 0.7,
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
      }}
      onClick={handleHideConnectionScreen}
    >
      {children}
    </Box>
  );
};

export default AddConnectionOverlay;
