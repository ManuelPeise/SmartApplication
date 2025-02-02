import { Box, Grid2, Typography } from "@mui/material";
import React from "react";
import { EmailClassificationModel, EmailTableColumn } from "../types";
import { colors } from "src/_lib/colors";
import { AutoSizer, Column, Table } from "react-virtualized";
import "react-virtualized/styles.css";
import { DropDownItem } from "src/_components/Input/Dropdown";
import EmailTablePlaceHolder from "./EmailTablePlaceHolder";

interface IProps {
  classifications: EmailClassificationModel[];
  columnDefinitions: EmailTableColumn<EmailClassificationModel>[];
  folderDropdownItems: DropDownItem[];
  handleItemsChanged: (
    partialState: Partial<EmailClassificationModel>,
    id: number
  ) => void;
}

const EmailClassificationTable: React.FC<IProps> = (props) => {
  const {
    classifications,
    columnDefinitions,
    folderDropdownItems,
    handleItemsChanged,
  } = props;

  const dataGetter = React.useCallback(
    ({ index }) => {
      return classifications[index];
    },
    [classifications]
  );

  const headerRowRenderer = React.useCallback(
    ({ className, columns, style }) => {
      return (
        <Box
          className={className}
          role="row"
          style={{ ...style, backgroundColor: colors.lighter, opacity: 0.6 }}
        >
          {columns}
        </Box>
      );
    },
    []
  );

  const cellHeaderRenderer = React.useCallback(
    (
      label: React.ReactNode,
      coldef: EmailTableColumn<EmailClassificationModel>
    ) => {
      return (
        <Box
          display="flex"
          minWidth={coldef.width}
          justifyContent={coldef.align}
        >
          <Typography variant="caption" sx={{ fontWeight: 600 }}>
            {label}
          </Typography>
        </Box>
      );
    },
    []
  );

  return (
    <Grid2 height="740px">
      <AutoSizer>
        {({ width, height }) => (
          <Table
            width={width}
            height={height - 50}
            rowStyle={{
              borderBottom: `1px solid ${colors.lighter}`,
            }}
            noRowsRenderer={() => <EmailTablePlaceHolder />}
            rowCount={classifications.length}
            rowHeight={50}
            headerHeight={60}
            overscanRowCount={10}
            headerRowRenderer={headerRowRenderer}
            rowGetter={dataGetter}
          >
            {columnDefinitions.map((coldef, index) => (
              <Column
                key={`col-${index}`}
                label={coldef.name}
                dataKey={coldef.name}
                width={coldef.width}
                headerStyle={{
                  fontSize: "1rem",
                  fontWeight: 600,
                }}
                style={{ width: coldef.width }}
                headerRenderer={({ label }) =>
                  cellHeaderRenderer(coldef.headerLabel, coldef)
                }
                cellDataGetter={({ rowData, dataKey }) => rowData[dataKey]}
                cellRenderer={({ columnIndex, rowIndex, rowData }) =>
                  coldef.component({
                    dataKey: coldef.name,
                    colIndex: columnIndex,
                    rowIndex: rowIndex,
                    model: rowData,
                    disabled: coldef.isReadonly,
                    columnDefinition: coldef,
                    dropdownItems: folderDropdownItems,
                    handleChange: handleItemsChanged,
                  })
                }
              />
            ))}
          </Table>
        )}
      </AutoSizer>
    </Grid2>
  );
};

export default EmailClassificationTable;
