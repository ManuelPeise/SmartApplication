import React from "react";
import SettingsLayout, {
  SettingsListItem,
} from "src/_components/_containers/SettingsLayout";
import { useEmailConfigurationContextProvider } from "src/_hooks/userContextProvider";
import { Box } from "@mui/material";
import { getDefaultEmailProviderConfiguration } from "./emailProviders";
import EmailProviderAccountConfigurationForm from "./_components/EmailProviderAccountConfigurationForm";
import { EmailProviderConfiguration } from "./types";

const EmailProviderConfigurationPage: React.FC = () => {
  const [selectedTab, setSelectedTab] = React.useState<number>(0);
  const { emailProviderConfigurations, isLoading, getResource } =
    useEmailConfigurationContextProvider();

  const listItems =
    React.useMemo((): SettingsListItem<EmailProviderConfiguration>[] => {
      if (emailProviderConfigurations != null) {
        const allConfigurations = [
          ...emailProviderConfigurations,
          getDefaultEmailProviderConfiguration(
            emailProviderConfigurations.length + 1,
            getResource("administration.labelAddConfiguration")
          ),
        ];
        return (
          allConfigurations?.map((config, index) => {
            const item: SettingsListItem<EmailProviderConfiguration> = {
              id: config.id,
              index: index,
              label: config.name,
              img: config.provider.logo,
              model: config,
              selected: selectedTab === index,
              onClick: setSelectedTab.bind(null, index),
            };
            return item;
          }) ?? []
        );
      }
      return null;
    }, [selectedTab, emailProviderConfigurations, getResource, setSelectedTab]);

  if (emailProviderConfigurations === null) {
    return null;
  }

  return (
    <SettingsLayout listitems={listItems}>
      <Box
        sx={{
          width: "100%",
          height: "100%",
          padding: 0,
        }}
      >
        {listItems &&
          listItems.map((item) => {
            if (item.index === selectedTab) {
              return (
                <EmailProviderAccountConfigurationForm
                  key={item.index}
                  tabIndex={item.index}
                  selectedTab={selectedTab}
                  isLoading={isLoading}
                  configuration={item.model}
                  getResource={getResource}
                />
              );
            } else {
              return null;
            }
          })}
      </Box>
    </SettingsLayout>
  );
};

export default EmailProviderConfigurationPage;
