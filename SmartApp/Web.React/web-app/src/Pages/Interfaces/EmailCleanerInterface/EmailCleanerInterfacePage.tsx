import React from "react";
import { EmailCleanerSettings } from "./types";
import { VerticalTabListListItem } from "src/_components/Lists/VerticalTabListMenu";
import { AlternateEmailRounded } from "@mui/icons-material";
import { useI18n } from "src/_hooks/useI18n";
import VerticalTabPageLayout from "src/_components/Layouts/VerticalTabPageLayout";
import EmailCleanerInterfaceTab from "./components/EmailCleanerInterfaceTab";
import { Grid2 } from "@mui/material";

const maxHeight = 740;

interface IProps {
  isLoading: boolean;
  data: EmailCleanerSettings[];
  handleUpdateSettings: (model: EmailCleanerSettings) => Promise<void>;
  handleImportData: (accountId: number) => Promise<void>;
}

const EmailCleanerInterfacePage: React.FC<IProps> = (props) => {
  const { isLoading, data, handleUpdateSettings, handleImportData } = props;

  const { getResource } = useI18n();
  const [selectedTab, setSelectedTab] = React.useState<number>(0);

  const vertivalTabItems = React.useMemo((): VerticalTabListListItem[] => {
    const items: VerticalTabListListItem[] = [];

    data.forEach((item, index) => {
      items.push({
        key: index,
        title: item.accountName,
        subTitle: item.emailAddress,
        icon: <AlternateEmailRounded />,
        onClick: setSelectedTab.bind(null, index),
      });
    });

    return items;
  }, [data]);

  return (
    <VerticalTabPageLayout
      containerId="email-cleaner-interface-container"
      isLoading={isLoading}
      selectedTab={selectedTab}
      pageTitle={getResource("interface.labelEmailCleaner")}
      tabItems={vertivalTabItems}
    >
      <Grid2
        height="inherit"
        sx={{
          scrollbarWidth: "none",
          msOverflowStyle: "none",
          overflow: "hidden",
        }}
      >
        {data.map((dataset, index) => (
          <EmailCleanerInterfaceTab
            tabindex={index}
            selectedTab={selectedTab}
            dataSet={dataset}
            maxHeight={maxHeight}
            handleUpdateSettings={handleUpdateSettings}
            handleImportData={handleImportData}
          />
        ))}
      </Grid2>
    </VerticalTabPageLayout>
  );
};

export default EmailCleanerInterfacePage;
