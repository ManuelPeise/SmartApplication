import React from "react";
import {
  EmailAccountInitializationSettingsProps,
  EmailAccountSettingsModel,
  EmailAccountSettingsProps,
} from "../types";
import { StatelessApi } from "src/_lib/_api/StatelessApi";
import { useAuth } from "src/_hooks/useAuth";
import { useComponentInitialization } from "src/_hooks/useComponentInitialization";
import SettingsLayout, {
  SettingsListItem,
} from "src/_components/Layouts/SettingsLayout";
import { Box } from "@mui/material";
import { useI18n } from "src/_hooks/useI18n";
import EmailAccountSettingsForm from "./Components/EmailAccountSettingsForm";
import { EmailProviderTypeEnum } from "src/_lib/_enums/EmailProviderTypeEnum";

const initializeAsync = async (
  token: string
): Promise<EmailAccountInitializationSettingsProps> => {
  const api = StatelessApi.create();

  const loadSettings = async (): Promise<EmailAccountSettingsModel[]> => {
    return api.get<EmailAccountSettingsModel[]>(
      { serviceUrl: "EmailAccountSettings/GetEmailAccountSettings" },
      token
    );
  };

  const onSave = async (model: EmailAccountSettingsModel) => {
    return api.post<EmailAccountSettingsModel>(
      {
        serviceUrl: "EmailAccountSettings/SaveEmailAccountSettings",
        body: model,
      },
      token
    );
  };

  const [settings] = await Promise.all([loadSettings()]);

  return {
    items: settings,
    onSave: onSave,
  };
};

const EmailAccountSettingsPageInitializationContainer: React.FC = () => {
  const { authenticationState } = useAuth();

  const { initProps, isInitialized } =
    useComponentInitialization<EmailAccountInitializationSettingsProps>(
      authenticationState.token,
      initializeAsync
    );

  const [accountSettings, setAccountSettings] = React.useState<
    EmailAccountSettingsModel[] | null
  >(initProps?.items);

  const handleAddAccount = React.useCallback(
    (
      account: EmailAccountSettingsModel,
      initModel: EmailAccountSettingsModel,
      index: number
    ) => {
      const accounts = accountSettings.slice();
      accounts[index] = account;

      setAccountSettings(accounts);
    },
    [accountSettings]
  );

  React.useEffect(() => {
    if (initProps?.items != null) {
      setAccountSettings(initProps?.items);
    }
  }, [initProps?.items]);

  if (!isInitialized || accountSettings == null) {
    return null;
  }

  return (
    <EmailAccountSettingsPageContainer
      authenticationState={authenticationState}
      items={accountSettings}
      handleAddAccount={handleAddAccount}
      onSave={initProps.onSave}
    />
  );
};

const EmailAccountSettingsPageContainer: React.FC<EmailAccountSettingsProps> = (
  props
) => {
  const { authenticationState, items, handleAddAccount, onSave } = props;
  const { getResource } = useI18n();

  const initializationModel = React.useMemo((): EmailAccountSettingsModel => {
    return {
      id: -1,
      userId: authenticationState.jwtData.userId,
      accountName: getResource("settings.labelNewAccount"),
      providerType: EmailProviderTypeEnum.None,
      server: "",
      port: 993,
      emailAddress: "",
      password: "",
      messageLog: null,
    };
  }, [authenticationState, getResource]);

  const [selectedAccountId, setSelectedAccountId] = React.useState<number>(0);

  const onSecectedAccountChanged = React.useCallback((id: number) => {
    setSelectedAccountId(id);
  }, []);

  const accountItems = React.useMemo((): EmailAccountSettingsModel[] => {
    const accountSettings = [...items];

    accountSettings.push(initializationModel);

    return accountSettings;
  }, [initializationModel, items]);

  const listItems = React.useMemo((): SettingsListItem[] => {
    if (!accountItems?.length) {
      return [
        {
          id: 0,
          label: getResource("settings.labelNewAccount"),
          description: getResource("settings.descriptionAccountItem"),
          selected: selectedAccountId === 0,
          readonly: selectedAccountId === 0,
          onSectionChanged: onSecectedAccountChanged,
        },
      ];
    }

    const listItems: SettingsListItem[] = accountItems?.map((item, index) => {
      return item.id === -1
        ? {
            id: index,
            label: item.accountName,
            description: getResource("settings.descriptionAddNewAccountItem"),
            selected: selectedAccountId === index,
            readonly: selectedAccountId === index,
            onSectionChanged: onSecectedAccountChanged,
          }
        : {
            id: index,
            label: item.accountName,
            description: getResource("settings.descriptionAccountItem").replace(
              "{account}",
              item.accountName
            ),
            selected: selectedAccountId === index,
            readonly: selectedAccountId === index,
            onSectionChanged: onSecectedAccountChanged,
          };
    });

    return listItems;
  }, [accountItems, selectedAccountId, onSecectedAccountChanged, getResource]);

  return (
    <Box width="100%" height="100%">
      <Box padding={4}>
        <SettingsLayout listItems={listItems} selectedItem={selectedAccountId}>
          <Box height="100%">
            {accountItems.map((acc, index) => (
              <EmailAccountSettingsForm
                key={index}
                index={index}
                selectedIndex={selectedAccountId}
                mode={acc.id === -1 ? "edit" : "view"}
                model={acc}
                initModel={initializationModel}
                onSave={onSave}
                updateAccountSettings={handleAddAccount}
              />
            ))}
          </Box>
        </SettingsLayout>
      </Box>
    </Box>
  );
};

export default EmailAccountSettingsPageInitializationContainer;
