import React from "react";
import SettingsPage from "./SettingsPage";
import { SettingsListItem } from "src/_components/Layouts/SettingsLayout";
import { useI18n } from "src/_hooks/useI18n";

const SettingsPageContainer: React.FC = () => {
  const { getResource } = useI18n();
  const [section, setSection] = React.useState<number>(0);

  const listItems = React.useMemo((): SettingsListItem[] => {
    return [
      {
        id: 0,
        label: getResource("settings.labelEmailAccountSettings"),
        description: getResource("settings.descriptionEmailAccountSettings"),
        selected: section === 0,
        readonly: false,
        onSectionChanged: setSection,
      },
    ];
  }, [section, getResource]);

  return <SettingsPage items={listItems} selectedSection={section} />;
};

export default SettingsPageContainer;
