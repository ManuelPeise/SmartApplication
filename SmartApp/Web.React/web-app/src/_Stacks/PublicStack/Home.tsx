import { Button, Grid2 } from "@mui/material";
import React from "react";
import { useNavigate } from "react-router-dom";

const Home: React.FC = () => {
  const navigate = useNavigate();
  return (
    <Grid2 height="100%">
      <Button onClick={navigate.bind(null, "/private")}>Private</Button>
      <Button onClick={navigate.bind(null, "/")}>Home</Button>
    </Grid2>
  );
};

export default Home;
