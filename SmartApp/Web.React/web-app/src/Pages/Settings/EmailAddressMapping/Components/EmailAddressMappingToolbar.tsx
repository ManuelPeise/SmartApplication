import { Box, Paper, Typography } from "@mui/material";
import React from "react";
import FilterDropdown from "src/_components/Filter/FilterDropdown";
import FilterTextInput from "src/_components/Filter/FilterTextInput";
import { useI18n } from "src/_hooks/useI18n";
import { FilterDropdownItem } from "src/_lib/_types/filter";
import { colors } from "src/_lib/colors";
import { EmailAddressMappingFilter } from "../../Types/EmailCleanerConfiguration";

interface IProps {
  domainDropdownItems: FilterDropdownItem[];
  searchText: string;
  selecteDomain: FilterDropdownItem;
  updateFilter: (partialFilter: Partial<EmailAddressMappingFilter>) => void;
}

const EmailAddressMappingToolbar: React.FC<IProps> = (props) => {
  const { domainDropdownItems, selecteDomain, searchText, updateFilter } =
    props;
  const { getResource } = useI18n();

  return (
    <Box padding={2}>
      <Paper elevation={4}>
        <Box
          maxWidth="100%"
          padding={2}
          paddingLeft={4}
          paddingRight={4}
          display="flex"
          flexDirection="row"
          alignItems="baseline"
          justifyContent="space-between"
        >
          <Box>
            <Typography
              variant="h4"
              color={colors.typography.blue}
              fontStyle="italic"
              letterSpacing={0.2}
            >
              {getResource("settings.labelEmailAddressMapping")}
            </Typography>
          </Box>
          <Box
            minWidth="600px"
            maxWidth="60%"
            display="flex"
            flexDirection="row"
            alignItems="baseline"
            gap={3}
          >
            <Box width="60%">
              <FilterTextInput
                fullWidth
                label={getResource("settings.labelAddressFilter")}
                filterText={searchText}
                onChange={(value) => updateFilter({ searchText: value })}
              />
            </Box>
            <Box width="40%">
              <FilterDropdown
                label="Test"
                fullWidth
                items={domainDropdownItems}
                selectedItem={selecteDomain}
                onChange={(x) => updateFilter({ domainIndex: x })}
              />
            </Box>
          </Box>
        </Box>
      </Paper>
    </Box>
  );
};

export default EmailAddressMappingToolbar;
