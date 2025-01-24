import React from "react";
import {
  EmailPrediction,
  PredictionRequest,
  SaveTrainingDataRequest,
  SpamClassificationPageData,
} from "./types";
import { useI18n } from "src/_hooks/useI18n";
import VerticalTabPageLayout from "src/_components/Layouts/VerticalTabPageLayout";
import {
  verticalTablistItemStyle,
  VerticalTabListListItem,
} from "src/_components/Lists/VerticalTabListMenu";
import { InfoRounded, TableChartRounded } from "@mui/icons-material";
import SpamClassificationPageBody from "./SpamClassificationPageBody";

interface IProps {
  isLoading: boolean;
  pageData: SpamClassificationPageData;
  saveTrainingData: (request: SaveTrainingDataRequest) => Promise<boolean>;
  handleTrainModel: () => Promise<void>;
  handlePredictEmail: (model: PredictionRequest) => Promise<EmailPrediction>;
}

const infoIndex = 0;
const datalistIndex = 1;

const SpamClassificationPage: React.FC<IProps> = (props) => {
  const {
    isLoading,
    pageData,
    saveTrainingData,
    handleTrainModel,
    handlePredictEmail,
  } = props;
  const { getResource } = useI18n();

  const [isModifiedState, setIsModifiedState] = React.useState<boolean>(false);

  const [selectedTab, setSelectedTab] = React.useState<number>(0);

  const handleSelectedTabChanged = React.useCallback((tabIndex: number) => {
    setSelectedTab(tabIndex);
  }, []);

  const vertivalTabItems = React.useMemo((): VerticalTabListListItem[] => {
    const items: VerticalTabListListItem[] = [];

    items.push({
      key: infoIndex,
      disabled: isModifiedState,
      title: getResource("administration.labelInfo"),
      subTitle: getResource("administration.descriptionSpamAiInfo"),
      icon: <InfoRounded style={verticalTablistItemStyle} />,
      onClick: handleSelectedTabChanged.bind(null, infoIndex),
    });

    items.push({
      key: datalistIndex,
      disabled: isModifiedState,
      title: getResource("administration.labelClassification"),
      subTitle: getResource("administration.descriptionSpamClassicication"),
      icon: <TableChartRounded style={verticalTablistItemStyle} />,
      onClick: handleSelectedTabChanged.bind(null, datalistIndex),
    });

    return items;
  }, [isModifiedState, getResource, handleSelectedTabChanged]);

  return (
    <VerticalTabPageLayout
      containerId="spam-classification-page-container"
      pageTitle={getResource("administration.labelSpamAdministration")}
      isLoading={isLoading}
      maxHeight={800}
      selectedTab={selectedTab}
      tabItems={vertivalTabItems}
    >
      <SpamClassificationPageBody
        selectedTab={selectedTab}
        domainData={pageData.domains}
        statistics={pageData.statistics}
        handleModifiedState={setIsModifiedState}
        saveTrainingData={saveTrainingData}
        handleTrainModel={handleTrainModel}
        handlePredictEmail={handlePredictEmail}
      />
    </VerticalTabPageLayout>
  );
};

export default SpamClassificationPage;
