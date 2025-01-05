import { Grid2 } from "@mui/material";
import React from "react";
import { useAccessRights } from "src/_hooks/useAccessRights";

const Home: React.FC = () => {
  const { getAccessRight } = useAccessRights();

  const right = getAccessRight("UserAdministration");

  console.log(right);
  return (
    <Grid2 height="100%">
      Test
      {/* <Button onClick={navigate.bind(null, "/private")}>Private</Button>
      <Button onClick={navigate.bind(null, "/")}>Home</Button> */}
    </Grid2>
  );
};

export default Home;
