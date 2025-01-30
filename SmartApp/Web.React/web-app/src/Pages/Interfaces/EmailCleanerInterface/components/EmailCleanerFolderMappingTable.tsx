import React from "react";
import { EmailFolderMappingTableCellData, FolderMapping } from "../types";
import { useI18n } from "src/_hooks/useI18n";
import { Grid2 } from "@mui/material";
import { DropDownItem } from "src/_components/Input/Dropdown";
import { AutoSizer, CellMeasurerCache, Column, Table } from "react-virtualized";
import "react-virtualized/styles.css";
import { colors } from "src/_lib/colors";
import {
  checkboxCell,
  dropdownCell,
  folderPredictionCell,
  labelCell,
} from "./EmailMappingDataCells";

interface IProps {
  mappings: FolderMapping[];
  targetFolderDropdownItems: DropDownItem[];
  handleChange: (partialModel: Partial<FolderMapping>, id: number) => void;
}

const EmailCleanerFolderMappingTable: React.FC<IProps> = (props) => {
  const { mappings, targetFolderDropdownItems, handleChange } = props;
  const { getResource } = useI18n();

  const columnDefinitions =
    React.useMemo((): EmailFolderMappingTableCellData[] => {
      return [
        {
          dataKey: "isActive",
          headerLabel: getResource("interface.labelIsActive"),
          align: "center",
          minWidth: 100,
          maxWidth: 100,
          columnHeight: 50,
          component: checkboxCell,
          handleChange: handleChange,
        },
        {
          dataKey: "domain",
          headerLabel: getResource("interface.labelDomain"),
          minWidth: 400,
          maxWidth: 400,
          columnHeight: 50,
          component: labelCell,
          align: "left",
        },
        {
          dataKey: "sourceFolder",
          headerLabel: getResource("interface.labelSourceFolder"),
          minWidth: 300,
          maxWidth: 300,
          columnHeight: 50,
          component: labelCell,
          align: "left",
        },
        {
          dataKey: "targetFolderId",
          headerLabel: getResource("interface.labelTargetFolder"),
          align: "left",
          minWidth: 300,
          maxWidth: 300,
          columnHeight: 50,
          component: dropdownCell,
          handleChange: handleChange,
        },
        {
          dataKey: "predictedTargetFolderId",
          headerLabel: getResource("interface.labelPredictedTargetFolder"),
          minWidth: 300,
          maxWidth: 300,
          columnHeight: 50,
          toolTipLabel: getResource("interface.labelUpdatePrediction"),
          component: folderPredictionCell,
          align: "left",
        },
      ];
    }, [getResource, handleChange]);

  const headerRowRenderer = React.useCallback(
    ({ className, columns, style }) => {
      return (
        <Grid2
          className={className}
          role="row"
          style={{ ...style, backgroundColor: colors.lighter }}
        >
          {columns}
        </Grid2>
      );
    },
    []
  );

  const headerRenderer = React.useCallback(
    (label: React.ReactNode, coldef: EmailFolderMappingTableCellData) => {
      return (
        <Grid2 minWidth={coldef.minWidth} alignContent={coldef.align}>
          {label}
        </Grid2>
      );
    },
    []
  );

  const _cache = new CellMeasurerCache({
    fixedWidth: true,
    minHeight: 50,
  });

  if (mappings == null || !mappings.length) {
    return null;
  }

  return (
    <Grid2 size={12} width="100%" height="100%">
      <AutoSizer>
        {({ width, height }) => (
          <Table
            style={{ scrollbarWidth: "none" }}
            width={width}
            height={height}
            headerHeight={40}
            rowHeight={60}
            rowCount={mappings?.length}
            rowStyle={{ borderBottom: `1px solid ${colors.lighter}` }}
            headerRowRenderer={({ className, columns, style }) =>
              headerRowRenderer({ className, columns, style })
            }
            rowGetter={({ index }) => mappings[index]} // Function to get data for each row
          >
            {columnDefinitions.map((colDef, index) => (
              <Column
                key={`col-${index}`}
                label={colDef.headerLabel}
                dataKey={colDef.dataKey}
                width={colDef.minWidth}
                style={{ alignContent: colDef.align }}
                headerStyle={{
                  fontSize: "1rem",
                  fontWeight: 600,
                }}
                cellRenderer={({ columnIndex, rowIndex, parent, rowData }) =>
                  colDef.component({
                    id: mappings[rowIndex].id,
                    rowIndex: rowIndex,
                    columnIndex: columnIndex,
                    parent: parent,
                    data: rowData,
                    columnDefinition: colDef,
                    targetFolderDropdownItems: targetFolderDropdownItems,
                    cache: _cache,
                  })
                }
                cellDataGetter={({ rowData, dataKey }) => rowData[dataKey]}
                headerRenderer={({ label }) => headerRenderer(label, colDef)}
              />
            ))}
          </Table>
        )}
      </AutoSizer>
    </Grid2>
  );
};

export default EmailCleanerFolderMappingTable;
