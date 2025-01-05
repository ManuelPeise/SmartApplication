import {
  Checkbox,
  Grid2,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Tooltip,
} from "@mui/material";
import React from "react";
import { LogMessage, LogmessageTypeEnum } from "./Types/logMessage";
import { LogTableColumnDefinition, LogTableFilter } from "./Types/logTable";
import { useI18n } from "src/_hooks/useI18n";
import { utils } from "src/_lib/utils";
import { DateFormatEnum } from "src/_lib/_enums/DateFormatEnum";
import FormButton from "src/_components/Buttons/FormButton";
import LogMessageFilter from "./Components/LogMessageFilter";

interface IProps {
  logMessages: LogMessage[];
  onDeleteMessages: (messageIds: number[]) => Promise<void>;
}

const LogPage: React.FC<IProps> = (props) => {
  const { logMessages, onDeleteMessages } = props;
  const { getResource } = useI18n();
  const [selectedLogMessages, setSelectedLogMessages] = React.useState<
    number[]
  >([]);
  const [filter, setFilter] = React.useState<LogTableFilter>({
    date: "",
    type: -1,
  });

  const tableColumns = React.useMemo((): LogTableColumnDefinition[] => {
    return [
      { field: "id", headerName: "Id", width: "auto" },
      { field: "timeStamp", headerName: "Timestamp", width: 300 },
      { field: "messageType", headerName: "Type", width: 200 },
      { field: "message", headerName: "Message", width: 400 },
      { field: "exceptionMessage", headerName: "Exeption", width: 600 },
    ];
  }, []);

  const getLogmessageType = React.useCallback(
    (type: LogmessageTypeEnum): string => {
      switch (type) {
        case LogmessageTypeEnum.info:
          return getResource("administration.labelLogmessageTypeInfo");
        case LogmessageTypeEnum.error:
          return getResource("administration.labelLogmessageTypeError");
        case LogmessageTypeEnum.criticalError:
          return getResource("administration.labelLogmessageTypeCriticalError");
      }
    },
    [getResource]
  );

  const toggleSelectAll = React.useCallback(
    (event: React.ChangeEvent<HTMLInputElement>) => {
      if (event.currentTarget.checked) {
        setSelectedLogMessages(logMessages.map((msg) => msg.id));
        return;
      } else {
        setSelectedLogMessages([]);
      }
    },
    [logMessages]
  );

  const onSelectMessage = React.useCallback(
    (msgId: number) => {
      const update = [...selectedLogMessages];

      if (update.includes(msgId)) {
        setSelectedLogMessages(update.filter((id) => id !== msgId));

        return;
      }

      update.push(msgId);
      setSelectedLogMessages(update);
    },
    [selectedLogMessages]
  );

  const onFilterChanged = React.useCallback(
    (filterUpdate: Partial<LogTableFilter>) => {
      setFilter({ ...filter, ...filterUpdate });
      setSelectedLogMessages([]);
    },
    [filter]
  );

  const handleDeleteMessages = React.useCallback(async () => {
    await onDeleteMessages(selectedLogMessages);
  }, [selectedLogMessages, onDeleteMessages]);

  const filteredLogMessages = React.useMemo((): LogMessage[] => {
    if (filter.date === "" && filter.type === -1) {
      return logMessages;
    }

    let messages: LogMessage[] = logMessages;

    if (filter.date.length > 0) {
      messages =
        messages.filter((msg) =>
          utils
            .getFormattedDate(
              msg.timeStamp,
              DateFormatEnum.DayMonthYearWithTimeStamp
            )
            .startsWith(filter.date)
        ) ?? [];
    }

    if (filter.type !== -1) {
      messages = messages.filter((msg) => msg.messageType === filter.type);
    }

    return messages;
  }, [logMessages, filter]);

  return (
    <Grid2
      display="flex"
      justifyContent="center"
      padding={2}
      width="100%"
      height="100%"
    >
      <Grid2
        display="flex"
        flexDirection="column"
        justifyContent="flex-start"
        padding={1}
        bgcolor="#fff"
        width="100%"
        height="100%"
        sx={{
          height: "auto",
        }}
      >
        <LogMessageFilter
          filter={filter}
          disabled={!logMessages?.length}
          onChange={onFilterChanged}
        />
        <TableContainer
          sx={{
            maxHeight: "600px",
            overflowY: "scroll",
            overflowX: "hidden",
          }}
        >
          <Table
            stickyHeader
            sx={{
              width: "100%",
            }}
          >
            <TableHead>
              <TableRow>
                {tableColumns.map((col, key) => {
                  if (col.field === "id") {
                    return (
                      <TableCell key={key} sx={{ width: col.width }}>
                        <Tooltip title={getResource("common.labelSelectAll")}>
                          <Checkbox
                            indeterminate={
                              selectedLogMessages.length > 0 &&
                              selectedLogMessages.length < logMessages.length
                            }
                            checked={
                              selectedLogMessages.length === logMessages.length
                            }
                            disabled={
                              !filteredLogMessages?.length ||
                              filteredLogMessages?.length === 0
                            }
                            onChange={toggleSelectAll}
                          />
                        </Tooltip>
                      </TableCell>
                    );
                  }
                  return (
                    <TableCell
                      key={key}
                      sx={{ width: col.width, overflowX: "hidden" }}
                    >
                      {col.headerName}
                    </TableCell>
                  );
                })}
              </TableRow>
            </TableHead>
            <TableBody>
              {filteredLogMessages?.length > 0 &&
                filteredLogMessages?.map((msg, index) => {
                  return (
                    <TableRow key={index}>
                      <TableCell>
                        <Tooltip title={msg.id}>
                          <Checkbox
                            onChange={onSelectMessage.bind(null, msg.id)}
                            checked={selectedLogMessages.includes(msg.id)}
                          />
                        </Tooltip>
                      </TableCell>
                      <TableCell>
                        {utils.getFormattedDate(
                          msg.timeStamp,
                          DateFormatEnum.DayMonthYearWithTimeStamp
                        )}
                      </TableCell>
                      <TableCell>
                        {getLogmessageType(msg.messageType)}
                      </TableCell>
                      <TableCell>{msg.message}</TableCell>
                      <TableCell>{msg.exceptionMessage}</TableCell>
                    </TableRow>
                  );
                })}
            </TableBody>
          </Table>
        </TableContainer>
        <Grid2
          display="flex"
          justifyContent="flex-end"
          padding={2}
          width="100%"
        >
          <FormButton
            label={getResource("administration.labelDeleteLogMessages")}
            disabled={selectedLogMessages.length === 0}
            onAction={handleDeleteMessages}
          />
        </Grid2>
      </Grid2>
    </Grid2>
  );
};

export default LogPage;
