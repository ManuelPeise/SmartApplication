import React from "react";
import {
  EmailAddressMappingEntry,
  EmailAddressMappingFilter,
} from "../../EmailCleanerTypes";
import { useI18n } from "src/_hooks/useI18n";
import GenericDataGrid from "src/_components/DataGrid/GenericDataGrid";
import DataGridCheckboxCell from "src/_components/DataGrid/DataGridCheckboxCell";
import DataGridLabelCell from "src/_components/DataGrid/DataGridLabelCell";
import EmailCleanerDataGridActionCell from "./EmailCleanerDataGridActionCell";
import { GenericDataGridColDef } from "src/_components/DataGrid/dataGrid";
import DataGridTextFieldCell from "src/_components/DataGrid/DataGridTextFieldCell";
import DataGridDropdownCell from "src/_components/DataGrid/DataGridDropdownCell";
import { EmailCleanerAction } from "src/_lib/_enums/EmailCleanerAction";
import DetailsView, { ButtonProps } from "src/_components/Layouts/DetailsView";
import { Box, Divider } from "@mui/material";
import EmailCleanerMappingFilter from "./EmailCleanerMappingFilter";
import { extractValues, groupByKey } from "src/_lib/utils";
import { DropDownItem } from "src/_components/Input/Dropdown";
import LoadingIndicator from "src/_components/Loading/LoadingIndicator";
import { debounce } from "lodash";

interface IProps {
  isLoading: boolean;
  mappings: EmailAddressMappingEntry[];
  maxGridHeight: number;
  handleUpdateEmailAddressMappings: (
    mappings: EmailAddressMappingEntry[]
  ) => Promise<void>;
}

interface IMappingProps extends IProps {
  originalMappings: EmailAddressMappingEntry[];
  maxGridHeight: number;
}

const EmailAddessMappingDataGrid: React.FC<IMappingProps> = (props) => {
  const {
    originalMappings,
    mappings,
    maxGridHeight,
    isLoading,
    handleUpdateEmailAddressMappings,
  } = props;

  const { getResource } = useI18n();

  React.useEffect(() => {
    setAddressMappings(originalMappings);
  }, [originalMappings]);

  const defaultDomainFilter = React.useMemo((): DropDownItem => {
    return {
      key: 0,
      label: getResource("emailCleaner.labelAllDomains"),
      disabled: false,
    };
  }, [getResource]);

  const [filter, setFilter] = React.useState<EmailAddressMappingFilter>({
    domain: defaultDomainFilter,
    searchText: "",
    groupByDomain: true,
  });

  const [modifiedRowIds, setModifiedRowIds] = React.useState<number[]>([]);
  const [addressMappings, setAddressMappings] =
    React.useState<EmailAddressMappingEntry[]>(mappings);

  const distinctBy = React.useCallback(
    (
      items: EmailAddressMappingEntry[],
      key: keyof EmailAddressMappingEntry
    ) => {
      const array: EmailAddressMappingEntry[] = [];
      const domainRecords = groupByKey<EmailAddressMappingEntry>(items, key);

      for (let domain in domainRecords) {
        const domainItems = items.filter((x) => x.domain === domain);
        if (domainItems.length) {
          array.push(domainItems[0]);
        }
      }

      return array;
    },
    []
  );

  const dataFilterFunction = React.useCallback(
    (itemsFilter: EmailAddressMappingFilter) => {
      if (itemsFilter.groupByDomain) {
        let domainItems = distinctBy(mappings, "domain");

        if (itemsFilter.domain.key !== 0) {
          domainItems = domainItems.filter(
            (x) =>
              x.domain.toLowerCase() === itemsFilter.domain.label.toLowerCase()
          );
        }

        if (itemsFilter.searchText !== "") {
          console.log("filter by searchtext");
          domainItems = domainItems.filter(
            (x) =>
              x.domain
                .toLowerCase()
                .startsWith(itemsFilter.searchText.toLowerCase()) ||
              x.sourceAddress
                .toLowerCase()
                .startsWith(itemsFilter.searchText.toLowerCase())
          );
        }
        setAddressMappings(domainItems);
      } else {
        const filteredMappings =
          itemsFilter.domain.key === 0
            ? mappings.filter((item) =>
                item.sourceAddress
                  .toLowerCase()
                  .startsWith(itemsFilter.searchText.toLowerCase())
              )
            : mappings.filter(
                (item) =>
                  item.sourceAddress
                    .toLowerCase()
                    .startsWith(itemsFilter.searchText.toLowerCase()) &&
                  item.domain.toLowerCase() ===
                    itemsFilter.domain.label.toLowerCase()
              );

        setAddressMappings(filteredMappings);
      }
    },
    [distinctBy, mappings]
  );

  const debouncedFilterLogic = React.useMemo(() => {
    return debounce(dataFilterFunction, 200);
  }, [dataFilterFunction]);

  const handleRevertChanges = React.useCallback(() => {
    setAddressMappings(originalMappings);
    setModifiedRowIds([]);
  }, [originalMappings]);

  const handleUpdateAddressMapping = React.useCallback(
    (mapping: EmailAddressMappingEntry) => {
      const modifiedMappingIndex = addressMappings.findIndex(
        (x) => x.id === mapping.id
      );

      if (filter.groupByDomain) {
        const changedMappings = addressMappings.filter(
          (mapping) =>
            mapping.domain === addressMappings[modifiedMappingIndex].domain
        );

        const mappingIds = changedMappings.map((mapping) => mapping.id);

        setAddressMappings([
          ...addressMappings.map((m) =>
            mappingIds.includes(m.id) ? { ...m, ...mapping, id: m.id } : m
          ),
        ]);

        const updatedRowIds = [...modifiedRowIds];

        mappingIds.forEach((id) => {
          if (!modifiedRowIds.includes(id)) {
            updatedRowIds.push(id);
          }
        });

        setModifiedRowIds(updatedRowIds);

        return;
      } else {
        setAddressMappings([
          ...addressMappings.map((m, index) =>
            index === modifiedMappingIndex ? { ...m, ...mapping } : m
          ),
        ]);

        if (!modifiedRowIds.includes(mapping.id)) {
          const updatedRowIds = [...modifiedRowIds];
          updatedRowIds.push(mapping.id);
          setModifiedRowIds(updatedRowIds);
        }
      }
    },
    [addressMappings, filter.groupByDomain, modifiedRowIds]
  );

  const handleIsSpamChanged = React.useCallback(
    (modelId: number, checked: boolean) => {
      const modifiedMapping = {
        ...addressMappings.find((x) => x.id === modelId),
        isSpam: checked,
      };

      handleUpdateAddressMapping(modifiedMapping);
    },
    [addressMappings, handleUpdateAddressMapping]
  );

  const handleActionChanged = React.useCallback(
    (modelId: number, value: number) => {
      const modifiedMapping = {
        ...addressMappings.find((x) => x.id === modelId),
        action: value,
      };

      handleUpdateAddressMapping(modifiedMapping);
    },
    [addressMappings, handleUpdateAddressMapping]
  );

  const handleIsActiveChanged = React.useCallback(
    (modelId: number, checked: boolean) => {
      const modifiedMapping = {
        ...addressMappings.find((x) => x.id === modelId),
        isActive: checked,
      };

      handleUpdateAddressMapping(modifiedMapping);
    },
    [addressMappings, handleUpdateAddressMapping]
  );

  const handleTargetFolderChanged = React.useCallback(
    (modelId: number, value: string) => {
      const modifiedMapping = {
        ...addressMappings.find((x) => x.id === modelId),
        targetFolder: value,
      };

      handleUpdateAddressMapping(modifiedMapping);
    },
    [addressMappings, handleUpdateAddressMapping]
  );

  const handleResetChangesOfRow = React.useCallback(
    (modelId: number) => {
      const originalMappingIndex = originalMappings.findIndex(
        (x) => x.id === modelId
      );

      setAddressMappings([
        ...addressMappings.map((m, index) =>
          index === originalMappingIndex
            ? {
                ...m,
                targetFolder:
                  originalMappings[originalMappingIndex]?.targetFolder ===
                  undefined
                    ? ""
                    : originalMappings[originalMappingIndex].targetFolder,
                ...originalMappings[originalMappingIndex],
              }
            : m
        ),
      ]);

      if (modifiedRowIds.includes(modelId)) {
        const updatedRowIds = [...modifiedRowIds].filter(
          (id) => id !== modelId
        );

        setModifiedRowIds(updatedRowIds);
      }
    },
    [addressMappings, modifiedRowIds, originalMappings]
  );

  const handleSaveMappings = React.useCallback(async () => {
    const modifiedMappings = addressMappings.filter((mapping) =>
      modifiedRowIds.includes(mapping.id)
    );

    await handleUpdateEmailAddressMappings(modifiedMappings).then(() => {
      setModifiedRowIds([]);
    });
  }, [addressMappings, handleUpdateEmailAddressMappings, modifiedRowIds]);

  const columnDefinitions =
    React.useMemo((): GenericDataGridColDef<EmailAddressMappingEntry>[] => {
      return [
        {
          field: "isActive",
          headerName: getResource("emailCleaner.labelIsActive"),
          disableColumnMenu: true,
          minWidth: 30,
          headerAlign: "center",
          align: "center",
          resizable: false,
          sortable: false,
          hideable: true,
          flex: 1,
          renderCell: (params) => {
            return (
              <DataGridCheckboxCell
                rowId={params.row.id}
                padding={1}
                checked={params.row.isActive}
                handleCheckedChanged={handleIsActiveChanged}
                disabled={isLoading}
                toolTipText={
                  params.row.isActive
                    ? getResource("emailCleaner.toolTipDisable")
                    : getResource("emailCleaner.toolTipEnable")
                }
              />
            );
          },
        },
        {
          field: "sourceAddress",
          headerName: getResource("emailCleaner.labelSourceAddress"),
          disableColumnMenu: true,
          minWidth: 200,
          editable: false,
          headerAlign: "left",
          align: "left",
          resizable: false,
          sortable: false,
          hideable: true,
          flex: 1,
          renderCell: (params) => {
            const value = params.row.sourceAddress;

            return (
              <DataGridLabelCell
                padding={1}
                value={value}
                toolTipText={value}
              />
            );
          },
        },
        {
          field: "subject",
          headerName: getResource("emailCleaner.labelSubject"),
          disableColumnMenu: true,
          minWidth: 400,
          editable: false,
          headerAlign: "left",
          align: "left",
          resizable: false,
          sortable: false,
          hideable: true,
          flex: 1,
          renderCell: (params) => {
            const value = params.row.subject;

            return (
              <DataGridLabelCell
                padding={1}
                value={value}
                toolTipText={value}
              />
            );
          },
        },
        {
          field: "predictedValue",
          headerName: getResource("emailCleaner.labelPredictedValue"),
          disableColumnMenu: true,
          minWidth: 50,
          editable: true,
          headerAlign: "center",
          align: "center",
          resizable: false,
          sortable: false,
          hideable: true,
          flex: 1,
          renderCell: (params) => {
            const value = !params.row.predictedValue?.length
              ? getResource("emailCleaner.labelNotAvailable")
              : params.row.predictedValue;

            return <DataGridLabelCell padding={1} value={value} />;
          },
        },
        {
          field: "isSpam",
          headerName: getResource("emailCleaner.labelIsSpam"),
          disableColumnMenu: true,
          minWidth: 60,
          headerAlign: "center",
          align: "center",
          resizable: false,
          sortable: false,
          hideable: true,
          flex: 1,
          renderCell: (params) => {
            return (
              <DataGridCheckboxCell
                rowId={params.row.id}
                padding={1}
                checked={params.row.isSpam}
                disabled={!params.row?.isActive || isLoading}
                handleCheckedChanged={handleIsSpamChanged}
                toolTipText={
                  params.row.isSpam
                    ? getResource("emailCleaner.tooltipMarkAsHam")
                    : getResource("emailCleaner.tooltipMarkAsSpam")
                }
              />
            );
          },
        },
        {
          field: "action",
          headerName: getResource("emailCleaner.labelCleanupAction"),
          disableColumnMenu: true,
          minWidth: 100,
          headerAlign: "left",
          align: "left",
          resizable: false,
          sortable: false,
          hideable: true,
          flex: 1,
          renderCell: (params) => {
            return (
              <DataGridDropdownCell
                rowId={params.row.id}
                padding={1}
                disabled={
                  !params.row.isActive || !params.row.isSpam || isLoading
                }
                value={params.row.action as number}
                items={[
                  {
                    key: 0,
                    label: getResource("emailCleaner.labelIgnore"),
                    disabled: params.row.action === EmailCleanerAction.Ignore,
                  },
                  {
                    key: 1,
                    label: getResource("emailCleaner.labelBackup"),
                    disabled: params.row.action === EmailCleanerAction.Backup,
                  },
                  {
                    key: 2,
                    label: getResource("emailCleaner.labelDelete"),
                    disabled: params.row.action === EmailCleanerAction.Delete,
                  },
                ]}
                handleChange={handleActionChanged}
              />
            );
          },
        },
        {
          field: "targetFolder",
          headerName: getResource("emailCleaner.labelTargetFolder"),
          disableColumnMenu: true,
          minWidth: 150,
          headerAlign: "left",
          align: "left",
          resizable: false,
          sortable: false,
          hideable: true,
          flex: 1,
          renderCell: (params) => {
            return (
              <DataGridTextFieldCell
                rowId={params.row.id}
                padding={1}
                disabled={
                  !params.row.isActive ||
                  !params.row.isSpam ||
                  params.row.action !== 1 ||
                  isLoading
                }
                value={params.row.targetFolder}
                label={getResource("emailCleaner.labelTargetFolder")}
                handleChange={handleTargetFolderChanged}
              />
            );
          },
        },
        {
          field: "accountId",
          headerName: getResource("emailCleaner.labelActions"),
          disableColumnMenu: true,
          headerAlign: "center",
          align: "center",
          editable: false,
          resizable: false,
          sortable: false,
          hideable: true,
          flex: 1,
          renderCell: (params) => {
            return (
              <EmailCleanerDataGridActionCell
                padding={1}
                rowHasChanges={modifiedRowIds.includes(params.row.id)}
                handleResetRow={handleResetChangesOfRow.bind(
                  null,
                  params.row.id
                )}
              />
            );
          },
        },
      ];
    }, [
      isLoading,
      modifiedRowIds,
      getResource,
      handleIsActiveChanged,
      handleIsSpamChanged,
      handleTargetFolderChanged,
      handleActionChanged,
      handleResetChangesOfRow,
    ]);

  const saveCancelButtonProps = React.useMemo((): ButtonProps[] => {
    return [
      {
        label: getResource("common.labelCancel"),
        disabled: !modifiedRowIds?.length,
        onAction: handleRevertChanges,
      },
      {
        label: getResource("common.labelSave"),
        disabled:
          !modifiedRowIds?.length ||
          addressMappings.some(
            (x) =>
              modifiedRowIds.includes(x.id) &&
              x.action === 1 &&
              x.targetFolder === ""
          ),
        onAction: handleSaveMappings,
      },
    ];
  }, [
    modifiedRowIds,
    addressMappings,
    getResource,
    handleRevertChanges,
    handleSaveMappings,
  ]);

  const handleFilterChanged = React.useCallback(
    (partialFilter: Partial<EmailAddressMappingFilter>) => {
      setFilter({ ...filter, ...partialFilter });
    },
    [filter]
  );

  const availableDomains = React.useMemo((): DropDownItem[] => {
    const items: DropDownItem[] = [defaultDomainFilter];
    const extratcedDomains = extractValues<string, EmailAddressMappingEntry>(
      mappings,
      "domain"
    ).sort((a, b) => (a < b ? -1 : 1));

    extratcedDomains.forEach((domain, index) => {
      const value = index + 1;
      items.push({
        key: value,
        label: domain,

        disabled: filter.domain.key === value,
      });
    });

    return items;
  }, [mappings, defaultDomainFilter, filter.domain.key]);

  React.useEffect(() => {
    debouncedFilterLogic(filter);
  }, [debouncedFilterLogic, filter]);

  return (
    <DetailsView
      saveCancelButtonProps={saveCancelButtonProps}
      additionalButtonProps={[]}
    >
      <Box
        width="100%"
        display="flex"
        flexDirection="column"
        gap={2}
        paddingBottom={1}
      >
        <LoadingIndicator isLoading={isLoading} />
        <EmailCleanerMappingFilter
          domains={availableDomains}
          groupByDisabled={modifiedRowIds.length > 0}
          filter={filter}
          handleFilterChanged={handleFilterChanged}
          handleResetFilter={() =>
            setFilter({
              searchText: "",
              domain: defaultDomainFilter,
              groupByDomain: filter.groupByDomain,
            })
          }
        />
        <Divider variant="middle" />
        <GenericDataGrid
          rowHeight={70}
          models={addressMappings}
          columnDefinitions={columnDefinitions}
          disableFocus={true}
          maxHeight={maxGridHeight}
        />
      </Box>
    </DetailsView>
  );
};

const EmailAddressMappingContainer: React.FC<IProps> = (props) => {
  const {
    mappings,
    maxGridHeight,
    isLoading,
    handleUpdateEmailAddressMappings,
  } = props;

  const originalStateRef = React.useRef([...mappings]);

  return (
    <EmailAddessMappingDataGrid
      originalMappings={originalStateRef.current}
      isLoading={isLoading}
      mappings={mappings}
      maxGridHeight={maxGridHeight}
      handleUpdateEmailAddressMappings={handleUpdateEmailAddressMappings}
    />
  );
};

export default EmailAddressMappingContainer;
