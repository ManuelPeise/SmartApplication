import { Checkbox, FormControlLabel, Grid2 } from "@mui/material";
import React from "react";
import { useI18n } from "src/_hooks/useI18n";
import FilterTextInput from "src/_components/Input/FilterTextInput";
import { EmailFolderMappingFilter } from "../types";

interface IProps {
  filter: EmailFolderMappingFilter;
  handleFilterChanged: (
    patialFilter: Partial<EmailFolderMappingFilter>
  ) => void;
}

const FolderMappingFilter: React.FC<IProps> = (props) => {
  const { filter, handleFilterChanged } = props;
  const { getResource } = useI18n();

  return (
    <Grid2
      size={12}
      height="inherit"
      width="100%"
      display="flex"
      flexDirection="row"
      justifyContent="space-around"
      alignItems="baseline"
      pr={4}
      gap={2}
    >
      <Grid2 height="inherit" maxWidth="300px">
        <FormControlLabel
          label={getResource("interface.labelShowOnlyIncativeMappings")}
          control={
            <Checkbox
              checked={filter.showOnlyInactive}
              onChange={(e) =>
                handleFilterChanged({
                  showOnlyInactive: e.currentTarget.checked,
                })
              }
            />
          }
        />
      </Grid2>
      <Grid2
        height="inherit"
        display="flex"
        alignItems="center"
        maxWidth="300px"
      >
        <FilterTextInput
          fullwidth
          label={getResource("interface.labelDomainFilter")}
          value={filter.domainFilter}
          onChange={(value) => handleFilterChanged({ domainFilter: value })}
          handleClearFilterText={() =>
            handleFilterChanged({ domainFilter: "" })
          }
        />
      </Grid2>
    </Grid2>
  );
};

export default FolderMappingFilter;
