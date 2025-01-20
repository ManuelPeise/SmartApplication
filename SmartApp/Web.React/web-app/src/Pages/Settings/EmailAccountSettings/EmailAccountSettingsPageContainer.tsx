import React from "react";
import { EmailAccountSettingsModel, EmailAccountSettingsProps } from "../types";
import { StatelessApi } from "src/_lib/_api/StatelessApi";
import { useAuth } from "src/_hooks/useAuth";
import SettingsLayout, {
  SettingsListItem,
} from "src/_components/Layouts/SettingsLayout";
import { Box } from "@mui/material";
import { useI18n } from "src/_hooks/useI18n";
import EmailAccountSettingsForm from "./Components/EmailAccountSettingsForm";
import { EmailProviderTypeEnum } from "src/_lib/_enums/EmailProviderTypeEnum";
import { useStatefulApiService } from "src/_hooks/useStatefulApiService";

const EmailAccountSettingsPageContainer: React.FC<EmailAccountSettingsProps> = (
  props
) => {
  const { authenticationState } = useAuth();
  const api = StatelessApi.create();
  const { getResource } = useI18n();

  const { data, sendPost, rebindData } = useStatefulApiService<
    EmailAccountSettingsModel[]
  >(
    api,
    {
      serviceUrl: "EmailAccountSettings/GetEmailAccountSettings",
    },
    authenticationState.token
  );

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

  const onSave = async (model: EmailAccountSettingsModel) => {
    return sendPost<boolean>({
      serviceUrl: "EmailAccountSettings/SaveEmailAccountSettings",
      body: model,
    }).then(async (res) => {
      if (res) {
        await rebindData();
      }
    });
  };

  const accountItems = React.useMemo((): EmailAccountSettingsModel[] => {
    const accountSettings = data != null ? [...data] : [];

    accountSettings.push(initializationModel);

    return accountSettings;
  }, [initializationModel, data]);

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

  if (data == null) {
    return null;
  }

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
                onSave={onSave}
              />
            ))}
          </Box>
        </SettingsLayout>
      </Box>
    </Box>
  );
};

export default EmailAccountSettingsPageContainer;
