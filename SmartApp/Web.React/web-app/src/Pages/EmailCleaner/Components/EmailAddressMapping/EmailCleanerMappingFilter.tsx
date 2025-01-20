import {
  Checkbox,
  FormControlLabel,
  Grid2,
  IconButton,
  Tooltip,
} from "@mui/material";
import React from "react";
import { useI18n } from "src/_hooks/useI18n";
import { EmailAddressMappingFilter } from "../../EmailCleanerTypes";
import FilterTextInput from "src/_components/Filter/FilterTextInput";
import { ClearRounded } from "@mui/icons-material";
import Dropdown, { DropDownItem } from "src/_components/Input/Dropdown";

interface IProps {
  filter: EmailAddressMappingFilter;
  groupByDisabled: boolean;
  domains: DropDownItem[];
  handleFilterChanged: (
    partialFilter: Partial<EmailAddressMappingFilter>
  ) => void;
  handleResetFilter: () => void;
}

const EmailCleanerMappingFilter: React.FC<IProps> = (props) => {
  const {
    filter,
    domains,
    groupByDisabled,
    handleFilterChanged,
    handleResetFilter,
  } = props;
  const { getResource } = useI18n();

  return (
    <Grid2
      container
      bgcolor="#f2f2f2"
      width="100%"
      size={12}
      padding={2}
      paddingRight={4}
      paddingLeft={4}
      display="flex"
      flexDirection="row"
      justifyContent="space-between"
      alignItems="flex-end"
      gap={2}
    >
      <Grid2 size={3}>
        <FilterTextInput
          fullWidth
          label={getResource("emailCleaner.labelSearch")}
          filterText={filter.searchText}
          disabled={!domains.length}
          onChange={(value) => handleFilterChanged({ searchText: value })}
        />
      </Grid2>
      <Grid2 size={3}>
        <Dropdown
          value={filter.domain.key}
          items={domains}
          maxHeight={600}
          fullWidth
          onChange={(value) =>
            handleFilterChanged({
              domain: domains.find((x) => x.key === value),
            })
          }
        />
      </Grid2>
      <Grid2 size={3}>
        <Tooltip
          title={getResource("emailCleaner.tooltipGroupByDomain")}
          children={
            <FormControlLabel
              label={getResource("emailCleaner.labelGroubByDomain")}
              control={
                <Checkbox
                  checked={filter.groupByDomain}
                  disabled={groupByDisabled}
                  onChange={(e) =>
                    handleFilterChanged({
                      groupByDomain: e.currentTarget.checked,
                    })
                  }
                />
              }
            />
          }
        />
      </Grid2>
      <Grid2
        size={2}
        display="flex"
        justifyContent="flex-end"
        alignItems="baseline"
        paddingRight={2}
        gap={2}
      >
        <Tooltip
          title={getResource("emailCleaner.labelClearAllFilters")}
          children={
            <IconButton
              size="medium"
              disabled={filter.domain.key === 0 && filter.searchText === ""}
              onClick={handleResetFilter}
            >
              <ClearRounded />
            </IconButton>
          }
        />
      </Grid2>
    </Grid2>
  );
};

export default React.memo(EmailCleanerMappingFilter);
