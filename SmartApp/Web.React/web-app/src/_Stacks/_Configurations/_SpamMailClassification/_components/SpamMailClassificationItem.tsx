import React from "react";
import { AiEmailTrainingData, SpamClassificationEnum } from "../spamTypes";
import { Box, ListItem, ListItemText } from "@mui/material";
import { useI18n } from "src/_hooks/useI18n";
import Dropdown, { DropDownItem } from "src/_components/_input/Dropdown";
import { colors } from "src/_lib/colors";

interface IProps {
  item: AiEmailTrainingData;
  handleModifiedItem: (item: AiEmailTrainingData) => void;
}

const SpamMailClassificationItem: React.FC<IProps> = (props) => {
  const { item, handleModifiedItem } = props;
  const { getResource } = useI18n();

  const classificationDropdownItems = React.useMemo((): DropDownItem[] => {
    const items: DropDownItem[] = [
      {
        key: SpamClassificationEnum.Ham,
        disabled: item.classification === SpamClassificationEnum.Ham,
        label: getResource("common.labelSelectHam"),
      },
      {
        key: SpamClassificationEnum.Spam,
        disabled: item.classification === SpamClassificationEnum.Spam,
        label: getResource("common.labelSelectSpam"),
      },
      {
        key: SpamClassificationEnum.Unknown,
        disabled: item.classification === SpamClassificationEnum.Unknown,
        label: getResource("common.labelSelectUnknown"),
      },
    ];

    return items;
  }, [item, getResource]);

  return (
    <ListItem
      divider
      disableGutters
      disablePadding
      sx={{
        display: "flex",
        flexDirection: "row",
        gap: 2,
        width: "100%",
        padding: ".5rem",
        paddingRight: "2rem",
        paddingLeft: "2rem",
        "&:hover": {
          backgroundColor: colors.background.lightgray,
        },
      }}
    >
      <Box width="20%">
        <ListItemText>{item.from}</ListItemText>
      </Box>
      <Box width="80%" textOverflow="ellipsis">
        <ListItemText>{item.subject}</ListItemText>
      </Box>
      <Box width="10%">
        <Dropdown
          value={item.classification}
          fullWidth
          items={classificationDropdownItems}
          onChange={(x) => handleModifiedItem({ ...item, classification: x })}
        />
      </Box>
    </ListItem>
  );
};

export default SpamMailClassificationItem;
