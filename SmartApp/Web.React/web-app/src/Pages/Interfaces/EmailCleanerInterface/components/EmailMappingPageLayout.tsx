import { DevicesFoldRounded, NavigateNextRounded } from "@mui/icons-material";
import { Box, Grid2, IconButton, Paper } from "@mui/material";
import React from "react";
import {
  EmailFolder,
  EmailTargetFolder,
  FolderMapping,
  FolderMappingUpdate,
  TargetFolderSectionState,
} from "../types";
import FolderCard from "./FolderCard";
import { colors } from "src/_lib/colors";
import EmailCleanerMappingTable from "./EmailCleanerFolderMappingTable";
import DetailsView, { ButtonProps } from "src/_components/Layouts/DetailsView";
import { DropDownItem } from "src/_components/Input/Dropdown";
import { useI18n } from "src/_hooks/useI18n";
import FolderMappingFilter from "./FolderMappingFilter";
import { isEqual } from "lodash";

interface IProps {
  mappings: FolderMapping[];
  folders: EmailTargetFolder[];
  settingsGuid: string;
  handleUpdate: (update: FolderMappingUpdate) => Promise<void>;
}

const EmailMappingPageLayout: React.FC<IProps> = (props) => {
  const { mappings, folders, settingsGuid, handleUpdate } = props;
  const { getResource } = useI18n();

  const [filter, setFilter] = React.useState<string>("");
  const [modifiedIds, setModifiedIds] = React.useState<number[]>([]);
  const [mappingData, setMappingData] = React.useState<FolderMapping[]>(
    mappings.sort((a, b) => (a.domain < b.domain ? -1 : 1))
  );

  const handleFilterChanged = React.useCallback((value: string) => {
    setFilter(value);
  }, []);

  const handleChange = React.useCallback(
    (partialModel: Partial<FolderMapping>, id: number) => {
      const modifiedItem = mappingData.find((x) => x.id === id);
      const update = { ...modifiedItem, ...partialModel };
      const ids = [...modifiedIds];

      setMappingData(mappingData.map((td) => (td.id === id ? update : td)));

      const originalMapping = mappings.find((x) => x.id === id);
      if (
        originalMapping.isActive !== update.isActive ||
        originalMapping.targetFolderId !== update.targetFolderId
      ) {
        if (!modifiedIds.includes(id)) {
          ids.push(id);
        }

        setModifiedIds(ids);
        return;
      }

      setModifiedIds(modifiedIds.filter((x) => x !== id));
    },
    [mappingData, mappings, modifiedIds]
  );

  //#region folders

  const targetFolderDropdownItems = React.useMemo((): DropDownItem[] => {
    return folders
      .map((folder) => {
        return {
          key: folder.id,
          label: getResource(`interface.${folder.resourceKey}`),
        };
      })
      .sort((a, b) => (a.label < b.label ? -1 : 1));
  }, [folders, getResource]);

  const availableEmailFolders = React.useMemo((): EmailFolder[] => {
    return targetFolderDropdownItems
      .map((item) => {
        return {
          folderId: item.key,
          folderName: item.label,
          inboxCount: mappingData.filter((x) => x.targetFolderId === item.key)
            .length,
        };
      })
      .sort((a, b) => (a.folderName < b.folderName ? -1 : 1));
  }, [targetFolderDropdownItems, mappingData]);

  const [folderSectionState, setFolderSectionState] =
    React.useState<TargetFolderSectionState>({
      expanded: true,
      width: "250px",
    });

  const handleFolderState = React.useCallback(() => {
    if (folderSectionState.expanded) {
      setFolderSectionState({ expanded: false, width: "50px" });
    } else {
      setFolderSectionState({ expanded: true, width: "250px" });
    }
  }, [folderSectionState]);

  //#endregion

  React.useEffect(() => {
    setMappingData(mappings);
  }, [mappings]);

  const saveCancelButtonProps = React.useMemo((): ButtonProps[] => {
    return [
      {
        label: getResource("common.labelCancel"),
        disabled: isEqual(mappings, mappingData),
        onAction: () => setMappingData(mappings),
      },
      {
        label: getResource("common.labelSave"),
        disabled: isEqual(mappings, mappingData),
        onAction: handleUpdate.bind(null, {
          settingsGuid: settingsGuid,
          mappings: mappingData.filter((x) => modifiedIds.includes(x.id)),
        }),
      },
    ];
  }, [
    getResource,
    mappings,
    mappingData,
    handleUpdate,
    settingsGuid,
    modifiedIds,
  ]);

  return (
    <Grid2
      height="100%"
      width="100%"
      display="flex"
      flexDirection="column"
      gap={2}
      p={2}
    >
      <Grid2 height="10%" size={12}>
        <Paper elevation={4} sx={{ height: "100%" }}>
          <FolderMappingFilter
            filter={filter}
            handleFilterChanged={handleFilterChanged}
          />
        </Paper>
      </Grid2>
      <Grid2
        maxHeight="inherit"
        size={12}
        display="flex"
        flexDirection="row"
        justifyContent="flex-end"
        gap={2}
      >
        <Grid2 minHeight={800} size="grow">
          <Paper elevation={4} sx={{ height: "100%" }}>
            <DetailsView
              justifyContent="center"
              saveCancelButtonProps={saveCancelButtonProps}
            >
              <Grid2
                p={2}
                minHeight={700}
                width="100%"
                display="flex"
                justifyContent="center"
                alignItems="flex-start"
              >
                <EmailCleanerMappingTable
                  mappings={mappingData.filter((mapping) =>
                    mapping.domain
                      .toLowerCase()
                      .startsWith(filter.toLowerCase())
                  )}
                  targetFolderDropdownItems={targetFolderDropdownItems}
                  handleChange={handleChange}
                />
              </Grid2>
            </DetailsView>
          </Paper>
        </Grid2>
        <Grid2 minHeight={700} size="auto">
          <Paper
            elevation={4}
            sx={{
              height: "100%",
              width: folderSectionState.width,
            }}
          >
            <Box
              width="100%"
              height="50px"
              display="flex"
              bgcolor={colors.lighter}
              justifyContent={
                folderSectionState.expanded ? "flex-start" : "center"
              }
              alignItems="center"
            >
              <IconButton onClick={handleFolderState}>
                {folderSectionState.expanded ? (
                  <NavigateNextRounded />
                ) : (
                  <DevicesFoldRounded />
                )}
              </IconButton>
            </Box>
            <Box
              width="100%"
              display="flex"
              flexDirection="column"
              gap={1}
              maxHeight={700}
              overflow="scroll"
              sx={{ scrollbarWidth: "none" }}
            >
              {availableEmailFolders.map((folder) => (
                <FolderCard
                  key={folder.folderId}
                  itemsCount={folder.inboxCount}
                  name={folder.folderName}
                  expanded={folderSectionState.expanded}
                />
              ))}
            </Box>
          </Paper>
        </Grid2>
      </Grid2>
    </Grid2>
  );
};

export default EmailMappingPageLayout;
