import { Box, Button, Typography } from "@mui/material";
import React from "react";
import { useNavigate } from "react-router-dom";
import LoadingIndicator from "src/_components/Loading/LoadingIndicator";
import { useI18n } from "src/_hooks/useI18n";
import confused from "src/_lib/_img/confused.jpg";
import { colors } from "src/_lib/colors";
import { browserRoutes } from "src/_lib/Router/RouterUtils";

interface IProps {
  allowReadEmails: boolean;
  isLoading: boolean;
  initializeMappings: () => Promise<void>;
}

const EmailAddressMappingPlaceholder: React.FC<IProps> = (props) => {
  const { allowReadEmails, isLoading, initializeMappings } = props;
  const navigate = useNavigate();
  const { getResource } = useI18n();
  return (
    <Box
      width="100%"
      height="100%"
      display="flex"
      justifyContent="space-between"
      alignItems="center"
      flexDirection="column"
      sx={{
        backgroundImage: `url(${confused})`,
        backgroundPosition: "center",
        backgroundRepeat: "no-repeat",
        backgroundSize: "cover",
      }}
    >
      <Box width="100%" maxHeight="10px">
        <LoadingIndicator isLoading={isLoading} />
      </Box>
      <Box
        width="100%"
        height="100%"
        display="flex"
        justifyContent="center"
        alignItems="center"
        flexDirection="column"
      >
        <Typography sx={{ fontSize: "1.5rem", color: "#fff", opacity: 0.6 }}>
          {getResource("settings.labelMissingEmailAddressMappings")}
        </Typography>
        <Box
          width="100%"
          display="flex"
          justifyContent="center"
          alignItems="center"
          padding={4}
          gap={2}
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
            disabled={isLoading}
            onClick={() => navigate(browserRoutes.emailCleanerSettings)}
          >
            {getResource("settings.labelBackToSettings")}
          </Button>
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
            disabled={!allowReadEmails || isLoading}
            onClick={initializeMappings}
          >
            {getResource("settings.labelInitializeMappings")}
          </Button>
        </Box>
      </Box>
    </Box>
  );
};

export default EmailAddressMappingPlaceholder;
