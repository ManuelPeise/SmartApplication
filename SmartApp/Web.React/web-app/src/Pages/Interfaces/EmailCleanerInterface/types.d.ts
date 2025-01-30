import { CellMeasurerCache } from "react-virtualized";
import { EmailProviderTypeEnum } from "src/_lib/_enums/EmailProviderTypeEnum";

export type EmailCleanerInterfaceConfigurationUiModel = {
  settingsGuid: string;
  userId: number;
  accountName: string;
  emailAddress: string;
  providerType: EmailProviderTypeEnum;
  unmappedDomains: number;
  connectionTestPassed: boolean;
  emailCleanerEnabled: boolean;
  useAiSpamPrediction: boolean;
  useAiTargetFolderPrediction: boolean;
  updatedBy?: string;
  updatedAt?: string;
  emails: EmailMappingModel[];
};

export type EmailMappingModel = {
  mappingId: number;
  addressEntityId: number;
  subjectEntityId: number;
  userId: number;
  userDefinedTargetFolder: string;
  predictedTargetFolder: string;
  userDefinedAsSpam: boolean;
  predictedAsSpam: boolean;
  fromAddress: string;
  subject: string;
};

export type EmailCleanerUpdateModel = {
  settingsGuid: string;
  emailCleanerEnabled: boolean;
  useAiSpamPrediction: boolean;
  useAiTargetFolderPrediction: boolean;
};

export type TargetFolderSectionState = {
  expanded: boolean;
  width: "250px" | "50px";
};

export type EmailFolder = {
  folderId: number;
  folderName: string;
  inboxCount: number;
};

export type EmailTargetFolder = {
  id: number;
  resourceKey: string;
};

export type FolderMapping = {
  id: number;
  userId: number;
  settingsGuid: string;
  sourceFolder: string;
  domain: string;
  targetFolderId: number;
  predictedTargetFolderId: number;
  isActive: boolean;
  shouldCleanup: boolean;
};

export type FolderMappingUpdate = {
  settingsGuid: string;
  mappings: FolderMapping[];
};

export type EmailFolderMappingData = {
  folders: EmailTargetFolder[];
  mappings: FolderMapping[];
};

export type EmailFolderMappingResponse = {
  settingsGuid: string;
  accountName: string;
  emailAddress: string;
  providerType: EmailProviderTypeEnum;
  mappingData: EmailFolderMappingData;
};

export type EmailFolderMappingTableCellData = {
  dataKey: keyof FolderMapping;
  headerLabel: string;
  align: "left" | "center" | "right";
  display?: "none";
  toolTipLabel?: string;
  minWidth?: number;
  maxWidth?: number;
  columnHeight: number;
  component: (props: FolderMappingColumnProps) => React.ReactNode;
  handleChange?: (partialModel: Partial<FolderMapping>, id: number) => void;
};

export type EmailFolderMappingFilter = {
  domainFilter: string;
};

export type FolderMappingColumnProps = {
  id: number;
  columnDefinition: EmailFolderMappingTableCellData;
  data: FolderMapping;
  targetFolderDropdownItems?: DropDownItem[];
  cache: CellMeasurerCache;
  parent: any;
  rowIndex: number;
  columnIndex: number;
};
