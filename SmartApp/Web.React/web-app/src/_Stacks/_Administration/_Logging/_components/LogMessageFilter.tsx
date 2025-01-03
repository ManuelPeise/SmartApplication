import { Grid2 } from "@mui/material";
import React from "react";
import FilterTextInput from "src/_components/_filter/FilterTextInput";
import { LogTableFilter } from "../../_types/logTable";
import { FilterDropdownItem } from "src/_lib/_types/filter";
import { LogmessageTypeEnum } from "../../_types/logMessage";
import { useI18n } from "src/_hooks/useI18n";
import FilterDropdown from "src/_components/_filter/FilterDropdown";

interface IProps {
  filter: LogTableFilter;
  disabled: boolean;
  onChange: (filterUpdate: Partial<LogTableFilter>) => void;
}

const LogMessageFilter: React.FC<IProps> = (props) => {
  const { filter, disabled, onChange } = props;
  const { getResource } = useI18n();

  const logMessageTypeDropdownItems =
    React.useMemo((): FilterDropdownItem[] => {
      return [
        {
          value: -1,
          label: getResource("administration.labelNone"),
          disabled: false,
        },
        {
          value: LogmessageTypeEnum.info,
          label: getResource("administration.labelLogmessageTypeInfo"),
          disabled: false,
        },
        {
          value: LogmessageTypeEnum.error,
          label: getResource("administration.labelLogmessageTypeError"),
          disabled: false,
        },
        {
          value: LogmessageTypeEnum.criticalError,
          label: getResource("administration.labelLogmessageTypeCriticalError"),
          disabled: false,
        },
      ];
    }, [getResource]);

  return (
    <Grid2
      width="100%"
      display="flex"
      justifyContent="space-around"
      alignItems="baseline"
      gap={2}
    >
      <Grid2 width="50%">
        <FilterTextInput
          filterText={filter.date}
          label={getResource("administration.labelTimeStampFilter")}
          onChange={(txt) => onChange({ date: txt })}
          disabled={disabled}
        />
      </Grid2>
      <Grid2 width="50%">
        <FilterDropdown
          fullWidth
          disabled={disabled}
          selectedItem={logMessageTypeDropdownItems.find(
            (x) => x.value === filter.type
          )}
          label={getResource("administration.labelLogMessageTypeFilter")}
          items={logMessageTypeDropdownItems}
          onChange={(type) => onChange({ type: type })}
        />
      </Grid2>
    </Grid2>
  );
};

export default LogMessageFilter;
