import { Grid2 } from "@mui/material";
import React from "react";
import { useI18n } from "src/_hooks/useI18n";
import FilterTextInput from "src/_components/Input/FilterTextInput";

interface IProps {
  filter: string;
  handleFilterChanged: (patialFilter: string) => void;
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
      justifyContent="flex-end"
      alignItems="center"
      pr={4}
      gap={2}
    >
      <Grid2
        height="inherit"
        display="flex"
        alignItems="center"
        maxWidth="300px"
      >
        <FilterTextInput
          fullwidth
          label={getResource("interface.labelDomainFilter")}
          value={filter}
          onChange={(value) => handleFilterChanged(value)}
          handleClearFilterText={() => handleFilterChanged("")}
        />
      </Grid2>
    </Grid2>
  );
};

export default FolderMappingFilter;
