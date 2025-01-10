import { Box, Button, Typography } from "@mui/material";
import React from "react";
import { useNavigate } from "react-router-dom";
import { useI18n } from "src/_hooks/useI18n";
import confused from "src/_lib/_img/confused.jpg";
import { colors } from "src/_lib/colors";
import { browserRoutes } from "src/_lib/Router/RouterUtils";

const EmailCleanerPlaceholder: React.FC = () => {
  const navigate = useNavigate();
  const { getResource } = useI18n();
  return (
    <Box
      width="100%"
      height="100%"
      display="flex"
      justifyContent="center"
      alignItems="center"
      sx={{
        backgroundImage: `url(${confused})`,
        backgroundPosition: "center",
        backgroundRepeat: "no-repeat",
        backgroundSize: "cover",
      }}
    >
      <Box>
        <Typography sx={{ fontSize: "1.5rem", color: "#fff", opacity: 0.6 }}>
          {getResource("settings.labelMissingEmailAccountConfiguration")}
        </Typography>
        <Box
          width="100%"
          display="flex"
          justifyContent="center"
          alignItems="center"
          padding={4}
        >
          <Button
            sx={{
              fontSize: "1.2rem",
              border: "2px solid transparent",
              borderRadius: "8px",
              color: colors.lightgray,
              opacity: 0.5,
              "&:hover": {
                borderColor: colors.lightgray,
                opacity: 0.8,
                transition: "all .5s ease-in-out",
              },
            }}
            onClick={() => navigate(browserRoutes.emailAccountSettings)}
          >
            {getResource("settings.labelConfigureEmailAccount")}
          </Button>
        </Box>
      </Box>
    </Box>
  );
};

export default EmailCleanerPlaceholder;
