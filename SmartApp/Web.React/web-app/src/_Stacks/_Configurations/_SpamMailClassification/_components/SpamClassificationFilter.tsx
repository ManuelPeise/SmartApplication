import React from "react";
import { StyledBox } from "src/_components/_styled/StyledBox";
import { EmailCassificationFilter } from "../spamTypes";
import Dropdown, { DropDownItem } from "src/_components/_input/Dropdown";
import FilterTextInput from "src/_components/_input/FilterTextInput";
import { useI18n } from "src/_hooks/useI18n";

interface IProps {
  filter: EmailCassificationFilter;
  classificationDropdownItems: DropDownItem[];
  handleFilterChanged: (
    partialState: Partial<EmailCassificationFilter>
  ) => void;
}

const SpamClassificationFilter: React.FC<IProps> = (props) => {
  const { filter, classificationDropdownItems, handleFilterChanged } = props;
  const { getResource } = useI18n();
  return (
    <StyledBox display="flex" flexDirection="row" alignItems="baseline" gap={4}>
      <StyledBox width="50%" padding={2}>
        <Dropdown
          fullWidth
          value={filter.classification}
          items={classificationDropdownItems}
          onChange={(value) => handleFilterChanged({ classification: value })}
        />
      </StyledBox>
      <StyledBox width="50%" padding={2}>
        <FilterTextInput
          label={getResource("common.labemAddressFilter")}
          fullwidth
          value={filter.address}
          handleClearFilterText={() => handleFilterChanged({ address: "" })}
          onChange={(value) => handleFilterChanged({ address: value })}
        />
      </StyledBox>
    </StyledBox>
  );
};

export default SpamClassificationFilter;
