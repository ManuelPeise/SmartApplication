import { Grid2 } from "@mui/material";
import React from "react";
import AutoCompleteFilter from "src/_components/Filter/AutoCompleteFilter";
import { DropDownItem } from "src/_components/Input/Dropdown";
import { useI18n } from "src/_hooks/useI18n";
import { EmailDomainModel } from "./types";

interface IProps {
  data: EmailDomainModel[];
  selectedDomainId: number;
  handleChange: (id: number) => void;
}

const EmailClassificationFilter: React.FC<IProps> = (props) => {
  const { data, selectedDomainId, handleChange } = props;
  const { getResource } = useI18n();

  const domainDropdownItems = React.useMemo((): DropDownItem[] => {
    const items: DropDownItem[] = [];

    data
      .sort((a, b) => (a.domainName < b.domainName ? -1 : 1))
      .forEach((domain) => {
        items.push({
          key: domain.id,
          label: domain.domainName,
          disabled: domain.id === selectedDomainId,
        });
      });

    return items;
  }, [data, selectedDomainId]);

  return (
    <Grid2
      bgcolor="#f2f2f2"
      padding={2}
      paddingRight={4}
      paddingLeft={4}
      minHeight="50px"
      marginBottom="10px"
      display="flex"
      justifyContent="flex-end"
      alignItems="baseline"
    >
      <AutoCompleteFilter
        componentKey="email-classification-domain-filter"
        minWidth={300}
        placeHolder={getResource("administration.labelSelectDomain")}
        options={domainDropdownItems}
        handleChange={handleChange}
      />
    </Grid2>
  );
};

export default EmailClassificationFilter;
