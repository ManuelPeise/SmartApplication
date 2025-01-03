import { Box, Button } from "@mui/material";
import React from "react";
import { useI18n } from "src/_hooks/useI18n";
import EstablishConnectionForm from "./Components/EstablishConnectionForm";
import {
  AccountSettings,
  EmailCleanupSettings,
} from "./Types/emailCleanupTypes";
import background from "./img/noConnection.jpg";

interface IProps {
  isLoading: boolean;
  showConnectionScreen: boolean;
  handleShowConnectionScreen: () => void;
  handleHideConnectionScreen: () => void;
  handleCheckConnection: (settings: AccountSettings) => Promise<boolean>;
  handleInitializeAccountInboxSettings: (
    settings: EmailCleanupSettings
  ) => Promise<EmailCleanupSettings>;
  handleSaveConnection: (settings: EmailCleanupSettings) => Promise<void>;
}

const NoConnectionPlaceholder: React.FC<IProps> = (props) => {
  const {
    isLoading,
    showConnectionScreen,
    handleShowConnectionScreen,
    handleHideConnectionScreen,
    handleCheckConnection,
    handleInitializeAccountInboxSettings,
    handleSaveConnection,
  } = props;

  const { getResource } = useI18n();
  return (
    <Box
      sx={{
        backgroundImage: `url(${background})`,
        backgroundPosition: "center",
        backgroundRepeat: "no-repeat",
        backgroundSize: "cover",
      }}
      width="100%"
      display="flex"
      justifyContent="center"
      alignItems="center"
    >
      {!showConnectionScreen ? (
        <Button
          sx={{
            padding: "0.6rem 1.1rem",
            backgroundColor: "#ffd11a",
            borderRadius: "24px",
            color: "#fff",
            fontSize: "1rem",
            fontWeight: "bold",
            opacity: 0.5,
            "&:hover": {
              cursor: "pointer",
              opacity: 1,
            },
          }}
          onClick={handleShowConnectionScreen}
        >
          {getResource("settings.labelEstablishConnection")}
        </Button>
      ) : (
        <EstablishConnectionForm
          isLoading={isLoading}
          handleHideConnectionScreen={handleHideConnectionScreen}
          handleCheckConnection={handleCheckConnection}
          handleInitializeAccountInboxSettings={
            handleInitializeAccountInboxSettings
          }
          handleSaveConnection={handleSaveConnection}
        />
      )}
    </Box>
  );
};

export default NoConnectionPlaceholder;
