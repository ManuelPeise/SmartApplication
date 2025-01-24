import { Grid2, Typography } from "@mui/material";
import React from "react";
import { useI18n } from "src/_hooks/useI18n";
import noEmailDomain from "src/_lib/_img/noEmailDomain.png";

interface IProps {
  maxHeight: number;
}

const NoDomainSelectedPlaceholder: React.FC<IProps> = (props) => {
  const { getResource } = useI18n();
  return (
    <Grid2
      height={props.maxHeight}
      display="flex"
      width="100%"
      justifyContent="center"
      alignItems="flex-end"
      paddingBottom="4rem"
      sx={{
        backgroundImage: `url(${noEmailDomain})`,
        backgroundSize: "cover",
        backgroundPosition: "center",
        backgroundRepeat: "no-repeat",
      }}
    >
      <Typography sx={{ fontSize: "3rem", color: "#fff" }}>
        {getResource("administration.labelSelectDomainFirst")}
      </Typography>
    </Grid2>
  );
};

export default NoDomainSelectedPlaceholder;
