import { Button } from "@mui/material";
import React from "react";
import { useNavigate } from "react-router-dom";

const Home: React.FC = () => {
  const navigate = useNavigate();
  return (
    <div>
      <Button onClick={navigate.bind(null, "/private")}>Private</Button>
      <Button onClick={navigate.bind(null, "/")}>Home</Button>
    </div>
  );
};

export default Home;
