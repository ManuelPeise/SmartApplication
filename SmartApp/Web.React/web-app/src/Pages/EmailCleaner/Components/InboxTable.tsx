import {
  Box,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TablePagination,
  TableRow,
  Typography,
} from "@mui/material";
import React from "react";
import { InboxTableColumnDefinition } from "../Types/table";
import { useI18n } from "src/_hooks/useI18n";
import {
  EmailCleanupSettings,
  FolderMappingEntry,
  SpamReport,
} from "../Types/emailCleanupTypes";
import InboxTableRow from "./InboxTableRow";

interface IProps {
  settings: EmailCleanupSettings;
  selectedFolderMapping: FolderMappingEntry;
  stickyHeader: boolean;
  handleUpdateState: (
    partialState: Partial<EmailCleanupSettings>
  ) => Promise<void>;
  handleReportAiTrainingData: (reportModel: SpamReport) => Promise<void>;
}

const InboxTable: React.FC<IProps> = (props) => {
  const {
    settings,
    selectedFolderMapping,
    stickyHeader,
    handleUpdateState,
    handleReportAiTrainingData,
  } = props;

  const { getResource } = useI18n();
  const [page, setPage] = React.useState<number>(0);
  const [rowsPerPage, setRowsPerPage] = React.useState(10);

  const handleChangePage = (event: unknown, newPage: number) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    setRowsPerPage(+event.target.value);
    setPage(0);
  };

  const handleUpdateBlacklist = React.useCallback(
    async (fromAddress: string) => {
      const settingsCopy = settings;

      if (
        settingsCopy.inboxConfiguration.blockListSettings.blockList.includes(
          fromAddress
        )
      ) {
        settingsCopy.inboxConfiguration.blockListSettings.blockList =
          settingsCopy.inboxConfiguration.blockListSettings.blockList.filter(
            (from) => from !== fromAddress
          );
      } else {
        settingsCopy.inboxConfiguration.blockListSettings.blockList.push(
          fromAddress
        );
      }

      await handleUpdateState(settingsCopy);
    },
    [settings, handleUpdateState]
  );

  const tableColumns = React.useMemo((): InboxTableColumnDefinition[] => {
    const columns: InboxTableColumnDefinition[] = [];

    columns.push({
      id: "date",
      label: getResource("settings.labelDate"),
      align: "left",
      width: "auto",
    });

    columns.push({
      id: "from",
      label: getResource("settings.labelFrom"),
      align: "left",
      width: "auto",
    });

    columns.push({
      id: "subject",
      label: getResource("settings.labelSubject"),
      align: "left",
      width: "auto",
    });

    columns.push({
      id: "ai-classification",
      label: getResource("settings.labelAiClassification"),
      align: "center",
      width: "auto",
    });

    columns.push({
      id: "action",
      label: getResource("settings.labelAction"),
      align: "center",
      width: "auto",
    });

    return columns;
  }, [getResource]);

  return (
    <Box>
      <Paper elevation={2}>
        <TableContainer sx={{ height: 680, maxHeight: 680 }}>
          <Table stickyHeader={stickyHeader}>
            <TableHead>
              <TableRow>
                {tableColumns.map((column) => (
                  <TableCell
                    key={column.id}
                    align={column.align}
                    sx={{
                      width: column.width,
                    }}
                  >
                    <Typography>{column.label}</Typography>
                  </TableCell>
                ))}
              </TableRow>
            </TableHead>
            <TableBody>
              {selectedFolderMapping &&
                selectedFolderMapping.classificationInformations
                  .slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
                  .map((dataSet, index) => (
                    <InboxTableRow
                      key={index}
                      data={dataSet}
                      isBlocked={settings.inboxConfiguration.blockListSettings.blockList.includes(
                        dataSet.from
                      )}
                      handleUpdateBlacklist={handleUpdateBlacklist}
                      handleReportAiTrainingData={handleReportAiTrainingData}
                    />
                  ))}
            </TableBody>
          </Table>
        </TableContainer>
        <TablePagination
          rowsPerPageOptions={[10, 25, 50]}
          rowsPerPage={rowsPerPage}
          component="div"
          count={selectedFolderMapping.classificationInformations.length}
          page={page}
          onPageChange={handleChangePage}
          onRowsPerPageChange={handleChangeRowsPerPage}
          labelRowsPerPage={getResource("settings.labelRowPerPage")}
        />
      </Paper>
    </Box>
  );
};

export default InboxTable;
