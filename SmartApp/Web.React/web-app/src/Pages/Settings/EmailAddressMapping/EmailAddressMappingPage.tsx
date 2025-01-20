import {
  Box,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Typography,
} from "@mui/material";
import React from "react";
import FormButton from "src/_components/Buttons/FormButton";
import { useI18n } from "src/_hooks/useI18n";
import EmailAddressMappingToolbar from "./Components/EmailAddressMappingToolbar";
import {
  EmailAccountMappings,
  EmailAddressMappingFilter,
} from "../Types/EmailCleanerConfiguration";
import { distinctBy, getCalculatedColumnWidth } from "src/_lib/utils";
import { FilterDropdownItem } from "src/_lib/_types/filter";

interface IProps {
  mappings: EmailAccountMappings;
}

const maxTableHeight = "650px";

const EmailAddressMappingPage: React.FC<IProps> = (props) => {
  const { mappings } = props;
  const { getResource } = useI18n();

  const [intermediateMappings, setIntermediateMappings] =
    React.useState<EmailAccountMappings>(mappings);

  React.useEffect(() => {
    if (mappings) {
      setIntermediateMappings(mappings);
    }
  }, [mappings]);

  const [filter, setFilter] = React.useState<EmailAddressMappingFilter>({
    domainIndex: 0,
    searchText: "",
  });

  const updateFilter = React.useCallback(
    (partialFilter: Partial<EmailAddressMappingFilter>) => {
      setFilter({ ...filter, ...partialFilter });
    },
    [filter]
  );

  const domainFilterDropdownItems = React.useMemo((): FilterDropdownItem[] => {
    const items: FilterDropdownItem[] = [
      {
        label: getResource("settings.labelShowAll"),
        value: 0,
        disabled: filter.domainIndex === -1,
      },
    ];

    const distinctedCollection = distinctBy(
      intermediateMappings?.mappings,
      (x) => x.domain
    );

    distinctedCollection
      .sort((a, b) => (a.domain < b.domain ? -1 : 1))
      .forEach((item, key) => {
        items.push({
          value: key + 1,
          label: item.domain,
          disabled: filter.domainIndex === key + 1,
        });
      });

    return items;
  }, [intermediateMappings?.mappings, filter, getResource]);

  const filteredMappings = React.useMemo(() => {
    let items = [...mappings.mappings];

    if (filter.searchText !== "") {
      items = items.filter((m) =>
        m.sourceAddress
          .toLowerCase()
          .startsWith(filter.searchText.toLowerCase())
      );
    }

    if (filter.domainIndex !== 0) {
      const selectedDomainIndex = domainFilterDropdownItems.findIndex(
        (x) => x.value === filter.domainIndex
      );

      items = items.filter(
        (m) => m.domain === domainFilterDropdownItems[selectedDomainIndex].label
      );
    }

    return items;
  }, [
    domainFilterDropdownItems,
    filter.domainIndex,
    filter.searchText,
    mappings.mappings,
  ]);

  const columnDefinition = React.useMemo(() => {
    return [
      {
        id: "sourceAddress",
        recourceKey: "settings.labelSourceAddress",
        align: "left",
        minWidth: getCalculatedColumnWidth(
          mappings.mappings,
          "sourceAddress",
          "px",
          3
        ),
        maxWidth: 100,
      },
      {
        id: "domain",
        recourceKey: "settings.labelDomain",
        align: "left",
        minWidth: getCalculatedColumnWidth(
          mappings.mappings,
          "domain",
          "px",
          3
        ),
        maxWidth: 80,
      },
      {
        id: "predictedValue",
        recourceKey: "settings.labelPridictedAs",
        align: "center",
        minWidth: 50,
        maxWidth: 50,
      },
      {
        id: "isSpam",
        recourceKey: "settings.labelIsSpam",
        align: "center",
        minWidth: 50,
        maxWidth: 50,
      },
      {
        id: "shouldCleanup",
        recourceKey: "settings.labelShouldCleanup",
        align: "center",
        minWidth: 50,
        maxWidth: 50,
      },
    ];
  }, [mappings.mappings]);

  return (
    <Box width="100%" height="100%">
      {/* toolbar */}
      <EmailAddressMappingToolbar
        searchText={filter.searchText}
        selecteDomain={domainFilterDropdownItems[filter.domainIndex]}
        domainDropdownItems={domainFilterDropdownItems}
        updateFilter={updateFilter}
      />
      <Box maxHeight={maxTableHeight + 50} minHeight="500px" padding={2}>
        <Paper
          sx={{ height: "100%", maxHeight: maxTableHeight + 50, width: "100%" }}
          elevation={2}
        >
          <Box
            minHeight={{
              xs: "auto",
              sm: "auto",
              md: "auto",
              lg: "650px",
              xl: "650px",
            }}
            height="100%"
            maxWidth="100%"
            padding={2}
            paddingLeft={4}
            paddingRight={4}
            display="flex"
            flexDirection="column"
            alignItems="baseline"
            justifyContent="flex-start"
          >
            <TableContainer sx={{ maxHeight: maxTableHeight }}>
              <Table stickyHeader>
                <TableHead>
                  <TableRow>
                    {columnDefinition.map((col, index) => (
                      <TableCell
                        key={index}
                        sx={{
                          minWidth: col.minWidth,
                          alignContent: col.align,
                          textOverflow: "ellipsis",
                        }}
                      >
                        <Typography sx={{ textAlign: col.align }}>
                          {getResource(col.recourceKey)}
                        </Typography>
                      </TableCell>
                    ))}
                  </TableRow>
                </TableHead>
                <TableBody>
                  {filteredMappings.map((mapping, rowIndex) => (
                    <TableRow key={rowIndex}>
                      {columnDefinition.map((col, colIndex) => (
                        <TableCell
                          sx={{ alignContent: col.align }}
                          key={colIndex}
                        >
                          <Typography sx={{ textAlign: col.align }}>
                            {mapping[col.id]?.toString()}
                          </Typography>
                        </TableCell>
                      ))}
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </TableContainer>
          </Box>
          {/* action container */}
          <Box
            padding={2}
            paddingRight={4}
            paddingLeft={4}
            display="flex"
            flexDirection="row"
            justifyContent="flex-end"
            gap={2}
          >
            <FormButton
              label={getResource("common.labelCancel")}
              //   disabled={isModified}
              onAction={() => {}}
            />
            <FormButton
              label={getResource("common.labelSave")}
              //   disabled={isModified}
              onAction={() => {}}
            />
          </Box>
        </Paper>
      </Box>
    </Box>
  );
};

export default EmailAddressMappingPage;
