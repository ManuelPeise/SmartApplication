import React from "react";
import { EmailAccountAiSettings, EmailAccountSettings } from "../types";
import { Grid2 } from "@mui/material";
import ListItemInput from "src/_components/Lists/ListItemInput";
import SwitchInput from "src/_components/Input/SwitchInput";
import { useI18n } from "src/_hooks/useI18n";

interface IProps {
  aiSettings: EmailAccountAiSettings;
  handleChange: (partialState: Partial<EmailAccountSettings>) => void;
}

const AccountAiSettings: React.FC<IProps> = (props) => {
  const { aiSettings, handleChange } = props;
  const { getResource } = useI18n();

  return (
    <Grid2 width="100%">
      <Grid2 paddingTop={2}>
        <ListItemInput
          label={getResource("interface.descriptionUseAiSpamPrediction")}
        >
          <SwitchInput
            checked={aiSettings.useAiSpamPrediction}
            handleChange={(e) =>
              handleChange({
                emailAccountAiSettings: {
                  ...aiSettings,
                  useAiSpamPrediction: e.target.checked,
                },
              })
            }
          />
        </ListItemInput>
      </Grid2>
      <Grid2 paddingTop={2}>
        <ListItemInput
          label={getResource(
            "interface.descriptionUseAiTargetFolderPrediction"
          )}
        >
          <SwitchInput
            checked={aiSettings.useAiTargetFolderPrediction}
            handleChange={(e) =>
              handleChange({
                emailAccountAiSettings: {
                  ...aiSettings,
                  useAiTargetFolderPrediction: e.target.checked,
                },
              })
            }
          />
        </ListItemInput>
      </Grid2>
    </Grid2>
  );
};

export default React.memo(AccountAiSettings);
