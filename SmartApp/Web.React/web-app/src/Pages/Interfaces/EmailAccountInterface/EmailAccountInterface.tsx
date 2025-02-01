import React from "react";
import VerticalTabPageLayout from "src/_components/Layouts/VerticalTabPageLayout";
import { VerticalTabListListItem } from "src/_components/Lists/VerticalTabListMenu";
import { useI18n } from "src/_hooks/useI18n";
import EmailAccountTab from "./components/EmailAccountTab";
import {
  EmailAccountConnectionTestRequest,
  EmailAccountSettings,
} from "./types";
import { AddRounded, AlternateEmailRounded } from "@mui/icons-material";
import { Grid2 } from "@mui/material";
import { EmailProviderTypeEnum } from "src/_lib/_enums/EmailProviderTypeEnum";

interface IProps {
  isLoading: boolean;
  data: EmailAccountSettings[];
  handleTestConnection: (
    request: EmailAccountConnectionTestRequest
  ) => Promise<boolean>;
  handleSaveConnection: (connection: EmailAccountSettings) => Promise<void>;
}
const maxHeight = 800;

const initialState: EmailAccountSettings = {
  userId: -1,
  accountId: -1,
  accountName: "",
  providerType: EmailProviderTypeEnum.None,
  imapServer: "",
  imapPort: 993,
  emailAddress: "",
  password: undefined,
  connectionTestPassed: false,
};

const EmailAccountInterface: React.FC<IProps> = (props) => {
  const { isLoading, data, handleTestConnection, handleSaveConnection } = props;
  const [selectedTab, setSelectedTab] = React.useState<number>(0);
  const { getResource } = useI18n();

  const handleSelectedTabChanged = React.useCallback((tabIndex: number) => {
    setSelectedTab(tabIndex);
  }, []);

  const verticalTabItems = React.useMemo((): VerticalTabListListItem[] => {
    const items: VerticalTabListListItem[] = [];

    data.forEach((item, index) => {
      if (item.accountName)
        items.push({
          key: index,
          icon: <AlternateEmailRounded />,
          title: item.accountName,
          subTitle: getResource("interface.descriptionAccount"),
          disabled: selectedTab === index,
          onClick: handleSelectedTabChanged.bind(null, index),
        });
    });

    items.push({
      key: items.length,
      icon: <AddRounded />,
      title: getResource("interface.labelAddAccount"),
      subTitle: getResource("interface.descriptionAddNewAccount"),
      disabled: selectedTab === items.length,
      onClick: handleSelectedTabChanged.bind(null, items.length),
    });
    return items;
  }, [data, selectedTab, getResource, handleSelectedTabChanged]);

  return (
    <VerticalTabPageLayout
      containerId="email-account-interface-container"
      pageTitle={getResource("interface.pageTitleEmailAccountInterface")}
      isLoading={isLoading}
      tabItems={verticalTabItems}
      selectedTab={selectedTab}
      maxHeight={maxHeight}
    >
      <Grid2
        height="inherit"
        sx={{
          scrollbarWidth: "none",
          msOverflowStyle: "none",
          overflow: "hidden",
        }}
      >
        {data?.map((connection, index) => (
          <EmailAccountTab
            state={connection}
            selectedTab={selectedTab}
            maxHeight={maxHeight}
            tabIndex={index}
            handleTestConnection={handleTestConnection}
            handleSaveConnection={handleSaveConnection}
          />
        ))}
        <EmailAccountTab
          state={initialState}
          selectedTab={selectedTab}
          maxHeight={maxHeight}
          tabIndex={data?.length}
          handleTestConnection={handleTestConnection}
          handleSaveConnection={handleSaveConnection}
        />
      </Grid2>
    </VerticalTabPageLayout>
  );
};

export default EmailAccountInterface;
