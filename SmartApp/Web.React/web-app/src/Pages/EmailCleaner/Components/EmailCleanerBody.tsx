import { Grid2, List, ListItemButton, Paper, Typography } from "@mui/material";
import React, { CSSProperties } from "react";
import {
  ConnectionTestModel,
  EmailAccountModel,
  EmailAddressMappingEntry,
  EmailCleanerListItem,
  EmailCleanerSettings,
  FolderSettings,
} from "../EmailCleanerTypes";
import {
  CategoryRounded,
  ManageAccountsRounded,
  SettingsRounded,
} from "@mui/icons-material";
import { colors } from "src/_lib/colors";
import AccountDetails from "./AccountDetails";
import { useI18n } from "src/_hooks/useI18n";
import EmailCleanerSettingsForm from "./EmailCleanerSettingsForm";
import EmailAddressMappingContainer from "./EmailAddressMapping/EmailAddressMappingDataGrid";

interface IProps {
  selectedAccount: EmailAccountModel;
  isLoading: boolean;
  handleGetUpdatedFolders: (accountId: number) => Promise<FolderSettings[]>;
  handleTestConnection: (model: ConnectionTestModel) => Promise<boolean>;
  handleSaveConnection: (model: EmailAccountModel) => Promise<boolean>;
  handleUpdateSettings: (model: EmailCleanerSettings) => Promise<void>;
  handleUpdateAccount: (model: EmailAccountModel) => Promise<void>;
  handleUpdateEmailAddressMappings: (
    mappings: EmailAddressMappingEntry[]
  ) => Promise<void>;
}

const listItemStyle: CSSProperties = {
  width: 40,
  height: 40,
  color: colors.darkgray,
};

const detailsIndex = 0;
const settingsIndex = 1;
const mappingIndex = 2;

const EmailCleanerBody: React.FC<IProps> = (props) => {
  const {
    selectedAccount,
    isLoading,
    handleTestConnection,
    handleSaveConnection,
    handleUpdateSettings,
    handleGetUpdatedFolders,
    handleUpdateAccount,
    handleUpdateEmailAddressMappings,
  } = props;
  const { getResource } = useI18n();
  const [selectedTab, setSelectedTab] = React.useState<number>(0);

  const handleSelectedTabChanged = React.useCallback((tabIndex: number) => {
    setSelectedTab(tabIndex);
  }, []);

  const vertivalTabItems = React.useMemo((): EmailCleanerListItem[] => {
    const items: EmailCleanerListItem[] = [];

    items.push({
      key: detailsIndex,
      title: getResource("emailCleaner.labelProvider"),
      subTitle: getResource("emailCleaner.labelProviderDetails"),
      icon: <ManageAccountsRounded style={listItemStyle} />,
      onClick: handleSelectedTabChanged.bind(null, detailsIndex),
    });

    items.push({
      key: settingsIndex,
      title: getResource("emailCleaner.labelSettings"),
      subTitle: getResource("emailCleaner.labelGeneralSettings"),
      icon: <SettingsRounded style={listItemStyle} />,
      disabled: selectedAccount == null || !selectedAccount?.settings,
      onClick: handleSelectedTabChanged.bind(null, settingsIndex),
    });

    items.push({
      key: mappingIndex,
      title: getResource("emailCleaner.labelMappings"),
      subTitle: getResource("emailCleaner.descriptionEmailAddressMapping"),
      icon: <CategoryRounded style={listItemStyle} />,
      disabled:
        selectedAccount == null ||
        !selectedAccount.settings.emailCleanerEnabled ||
        !selectedAccount?.emailAddressMappings?.length,
      onClick: handleSelectedTabChanged.bind(null, mappingIndex),
    });

    return items;
  }, [selectedAccount, handleSelectedTabChanged, getResource]);

  return (
    <Grid2 id="email-cleaner-body" size={12} minHeight="750px">
      <Paper sx={{ height: "100%", minWidth: "900px" }} elevation={4}>
        <Grid2 height="100%" size={12} display="flex" flexDirection="row">
          {/* vertical tab menu */}
          <Grid2
            id="email-cleaner-side-menu"
            size={2}
            height="100%"
            minWidth="250px"
            sx={{ opacity: 0.8 }}
          >
            <List
              disablePadding
              sx={{
                height: "100%",
                width: "100%",
                borderRight: `1px solid ${colors.lighter}`,
              }}
            >
              {vertivalTabItems.map((item) => (
                <ListItemButton
                  key={item.key}
                  sx={{
                    width: "100%",
                  }}
                  selected={item.key === selectedTab}
                  disabled={item.disabled || item.key === selectedTab}
                  onClick={item.onClick}
                >
                  <Grid2 size={12} display="flex">
                    <Grid2
                      size={2}
                      display="flex"
                      justifyContent="center"
                      alignItems="center"
                    >
                      {item?.icon}
                    </Grid2>
                    <Grid2 size={10} paddingLeft={2}>
                      <Typography sx={{ fontSize: "1rem" }}>
                        {item.title}
                      </Typography>
                      <Typography
                        style={{
                          color: colors.typography.darkgray,
                          fontSize: ".9rem",
                        }}
                      >
                        {item.subTitle}
                      </Typography>
                    </Grid2>
                  </Grid2>
                </ListItemButton>
              ))}
            </List>
          </Grid2>
          {/* tab container */}
          <Grid2 id="email-cleaner-details-container" size={10} p={2}>
            {selectedTab === detailsIndex && (
              <AccountDetails
                account={selectedAccount}
                isLoading={isLoading}
                handleTestConnection={handleTestConnection}
                handleSaveConnection={handleSaveConnection}
                handleUpdateAccount={handleUpdateAccount}
              />
            )}
            {selectedTab === settingsIndex && (
              <EmailCleanerSettingsForm
                isLoading={isLoading}
                accountId={selectedAccount.id}
                emailAddress={selectedAccount.emailAddress}
                providerType={selectedAccount.providerType}
                settings={selectedAccount.settings}
                handleUpdateSettings={handleUpdateSettings}
                handleGetUpdatedFolders={handleGetUpdatedFolders}
              />
            )}
            {selectedTab === mappingIndex && (
              <EmailAddressMappingContainer
                maxGridHeight={500}
                isLoading={isLoading}
                mappings={selectedAccount.emailAddressMappings}
                handleUpdateEmailAddressMappings={
                  handleUpdateEmailAddressMappings
                }
              />
            )}
          </Grid2>
        </Grid2>
      </Paper>
    </Grid2>
  );
};

export default EmailCleanerBody;
