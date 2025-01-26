import { Grid2 } from "@mui/material";
import React from "react";
import { useParams } from "react-router-dom";

const EmailDomainMappingContainer: React.FC = () => {
  const { id } = useParams();

  return <Grid2>{`Received settings guid: ${id}`}</Grid2>;
};

export default EmailDomainMappingContainer;
