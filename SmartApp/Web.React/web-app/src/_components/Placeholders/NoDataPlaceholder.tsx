import { Button, Grid2, Typography } from "@mui/material";
import React from "react";
import { useNavigate } from "react-router-dom";
import backgroundImg from "src/_lib/_img/confused.jpg";

interface IProps {
  buttonLabel: string;
  infoText: string;
  route: string;
}

const NoDataPlaceholder: React.FC<IProps> = (props) => {
  const { buttonLabel, infoText, route } = props;

  const navigate = useNavigate();

  return (
    <Grid2
      height="100%"
      width="100%"
      display="flex"
      justifyContent="center"
      flexDirection="column"
      sx={{
        backgroundImage: `url(${backgroundImg})`,
        backgroundPosition: "center",
        backgroundSize: "cover",
      }}
    >
      <Typography
        sx={{
          width: "100%",
          position: "absolute",
          top: 200,
          textAlign: "center",
          fontSize: "2rem",
          color: "#fff",
          opacity: 0.7,
          whiteSpace: "break-spaces",
          lineHeight: "4rem",
        }}
      >
        {infoText}
      </Typography>
      <Grid2
        sx={{
          width: "100%",
          position: "absolute",
          bottom: 120,
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
        }}
      >
        <Button
          sx={{
            color: "#fff",
            fontSize: "1.5rem",
            border: "1px solid transparent",
            padding: "0 10px 0 10px",
            opacity: 0.5,
            "&:hover": {
              opacity: 0.8,
              cursor: "pointer",
              border: "1px solid #fff",
            },
          }}
          onClick={() => navigate(route)}
        >
          {buttonLabel}
        </Button>
      </Grid2>
    </Grid2>
  );
};

export default NoDataPlaceholder;
