import React from "react";
import { useApi } from "src/_hooks/useApi";
import { EmailAccountSettings } from "../_types/Settings";
import { Grid2, List } from "@mui/material";
import { useI18n } from "src/_hooks/useI18n";
import SettingsListItemButton from "./SettingsListItemButton";
import EmailAccountServerSettingsDialog from "./EmailAccountServerSettingsDialog";

const tabindex = 0;
interface IProps {
  selectedTab: number;
}

const EmailCleanupSettingsTab: React.FC<IProps> = (props) => {
  const { selectedTab } = props;
  const { getResource } = useI18n();
  const [settingsDialogOpen, setSettingsDialogOpen] =
    React.useState<boolean>(false);

  const { data } = useApi<EmailAccountSettings>({
    requestUrl: "EmailCleanupModule/GetEmailCleanupSettings",
    isPrivate: true,
    parameters: null,
    initialLoad: selectedTab !== tabindex,
  });

  if (selectedTab !== tabindex) {
    return null;
  }

  return (
    <Grid2 width="100%" padding={4}>
      <List sx={{ width: "100%" }}>
        <SettingsListItemButton
          btnLabel={getResource("settings.labelSettings")}
          description={getResource("settings.descriptionEmailCleanupSettings")}
          onAction={setSettingsDialogOpen.bind(null, true)}
          disabled={settingsDialogOpen}
        />
      </List>
      <Grid2>Mapping</Grid2>
      <EmailAccountServerSettingsDialog
        open={settingsDialogOpen}
        data={data as EmailAccountSettings}
        onAction={setSettingsDialogOpen.bind(null, false)}
        onCancel={setSettingsDialogOpen.bind(null, false)}
      />
    </Grid2>
  );
};

export default EmailCleanupSettingsTab;
