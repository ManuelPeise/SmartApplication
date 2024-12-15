import React from "react";
import SettingsPage from "./SettingsPage";
import EmailCleanupSettingsTab from "./_components/EmailCleanupSettingsTab";

const SettingsPageContainer: React.FC = () => {
  const [selectedTab, setSelectedTab] = React.useState<number>(0);

  const onSelectedTabChanged = React.useCallback((tabIndex: number) => {
    setSelectedTab(tabIndex);
  }, []);

  return (
    <SettingsPage
      selectedTab={selectedTab}
      onSelectedTabChanged={onSelectedTabChanged}
    >
      <EmailCleanupSettingsTab selectedTab={selectedTab} />
    </SettingsPage>
  );
};

export default SettingsPageContainer;
