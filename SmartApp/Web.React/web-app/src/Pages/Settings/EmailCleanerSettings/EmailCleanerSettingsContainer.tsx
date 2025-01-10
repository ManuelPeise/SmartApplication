import React from "react";
import { StatelessApi } from "src/_lib/_api/StatelessApi";
import {
  EmailCleanerConfiguration,
  EmailCleanerSettings,
} from "./Types/EmailCleanerConfiguration";
import { useStatefulApiService } from "src/_hooks/useStatefulApiService";
import { useAuth } from "src/_hooks/useAuth";
import { DropDownItem } from "src/_components/Input/Dropdown";
import EmailCleanerPlaceholder from "./EmailCleanerPlaceHolder";
import EmailCleanerPage from "./EmailCleanerPage";

const EmailCleanerSettingsContainer: React.FC = () => {
  const { authenticationState } = useAuth();
  const api = StatelessApi.create();

  const [selectedAccount, setSelectedAccount] = React.useState<number>(0);

  const { isLoading, data, sendPost, rebindData } =
    useStatefulApiService<EmailCleanerConfiguration>(
      api,
      {
        serviceUrl: "EmailCleanupSettings/GetEmailCleanerSettings",
        parameters: { loadMappings: "false" },
      },
      authenticationState.token
    );

  const handleSaveSettings = React.useCallback(
    async (settings: EmailCleanerSettings) => {
      await sendPost<boolean>({
        serviceUrl: "EmailCleanupSettings/SaveOrUpdateEmailCleanerSettings",
        body: settings,
      }).then(async (res) => {
        if (res) {
          await rebindData();
        }
      });
    },
    [sendPost, rebindData]
  );

  const accountDropDownItems = React.useMemo((): DropDownItem[] => {
    const items: DropDownItem[] = [];

    data?.accounts.forEach((item, index) => {
      items.push({
        key: index,
        disabled: selectedAccount === index,
        label: item.accountName,
      });
    });

    return items;
  }, [data?.accounts, selectedAccount]);

  if (isLoading || !data?.settings == null) {
    return null;
  }

  if (data == null) {
    return <EmailCleanerPlaceholder />;
  }

  console.log(data);
  return (
    <EmailCleanerPage
      accountDropdownItems={accountDropDownItems}
      selectedAccount={selectedAccount}
      isLoading={isLoading}
      settings={data.settings[selectedAccount]}
      handleAccountChanged={setSelectedAccount}
      handleSaveSettings={handleSaveSettings}
    />
  );
};

export default EmailCleanerSettingsContainer;
