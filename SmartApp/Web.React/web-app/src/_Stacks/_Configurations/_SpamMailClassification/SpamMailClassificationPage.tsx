import React from "react";
import {
  AiEmailTrainingData,
  EmailCassificationFilter,
  SpamClassificationEnum,
} from "./spamTypes";
import { StyledBox } from "src/_components/_styled/StyledBox";
import { List, Paper } from "@mui/material";
import FilterLayout from "src/_components/_containers/FilterLayout";
import SpamClassificationFilter from "./_components/SpamClassificationFilter";
import { DropDownItem } from "src/_components/_input/Dropdown";
import { useI18n } from "src/_hooks/useI18n";
import FormButton from "src/_components/_buttons/FormButton";
import SpamMailClassificationItem from "./_components/SpamMailClassificationItem";
import LoadingIndicator from "src/_components/_loading/LoadingIndicator";

interface IProps {
  originalData: AiEmailTrainingData[];
  isLoading: boolean;
  handleSave: (items: AiEmailTrainingData[]) => Promise<void>;
}

const SpamMailClassificationPage: React.FC<IProps> = (props) => {
  const { originalData, isLoading, handleSave } = props;
  const { getResource } = useI18n();

  const [items, setItems] = React.useState<AiEmailTrainingData[]>(originalData);

  const [filter, setFilter] = React.useState<EmailCassificationFilter>({
    address: "",
    classification: SpamClassificationEnum.All,
  });

  const classificationDropdownItems = React.useMemo((): DropDownItem[] => {
    const items: DropDownItem[] = [
      {
        key: SpamClassificationEnum.All,
        disabled: filter.classification === SpamClassificationEnum.All,
        label: getResource("common.labelSelectAll"),
      },
      {
        key: SpamClassificationEnum.Ham,
        disabled: filter.classification === SpamClassificationEnum.Ham,
        label: getResource("common.labelSelectHam"),
      },
      {
        key: SpamClassificationEnum.Spam,
        disabled: filter.classification === SpamClassificationEnum.Spam,
        label: getResource("common.labelSelectSpam"),
      },
      {
        key: SpamClassificationEnum.Unknown,
        disabled: filter.classification === SpamClassificationEnum.Unknown,
        label: getResource("common.labelSelectUnknown"),
      },
    ];

    return items;
  }, [filter.classification, getResource]);

  const filteredData = React.useMemo((): AiEmailTrainingData[] => {
    let filterItems: AiEmailTrainingData[] = [...items];

    if (filter.classification === SpamClassificationEnum.Unknown) {
      filterItems = filterItems.filter(
        (x) => x.classification === SpamClassificationEnum.Unknown
      );
    }

    if (filter.classification === SpamClassificationEnum.Ham) {
      filterItems = filterItems.filter(
        (x) => x.classification === SpamClassificationEnum.Ham
      );
    }

    if (filter.classification === SpamClassificationEnum.Spam) {
      filterItems = filterItems.filter(
        (x) => x.classification === SpamClassificationEnum.Spam
      );
    }

    if (filter.address !== "") {
      filterItems = filterItems.filter((x) =>
        x.from.toLowerCase().startsWith(filter.address.toLowerCase())
      );
    }
    return filterItems;
  }, [filter, items]);

  const isModified = React.useMemo(() => {
    return items.some((item) => {
      const index = originalData.findIndex((x) => x.id === item.id);

      return originalData[index].classification !== item.classification;
    });
  }, [items, originalData]);

  const handleFilterChanged = React.useCallback(
    (partialFilter: Partial<EmailCassificationFilter>) => {
      setFilter({ ...filter, ...partialFilter });
    },
    [filter]
  );

  const handleCancel = React.useCallback(() => {
    setItems(originalData);
  }, [originalData]);

  const handleModifiedItem = React.useCallback(
    (item: AiEmailTrainingData) => {
      const originalItem = originalData.find((x) => x.id === item.id) ?? null;
      const itemsCopy: AiEmailTrainingData[] = [...items];

      if (item.classification !== originalItem.classification) {
        const index = itemsCopy.findIndex((x) => x.id === item.id);
        itemsCopy[index] = item;
        setItems(itemsCopy);
      }
    },
    [items, originalData]
  );

  const onSave = React.useCallback(async () => {
    await handleSave(items);
  }, [items, handleSave]);

  return (
    <FilterLayout>
      <Paper elevation={2} style={{ padding: "8px", position: "sticky" }}>
        <SpamClassificationFilter
          filter={filter}
          classificationDropdownItems={classificationDropdownItems}
          handleFilterChanged={handleFilterChanged}
        />
      </Paper>
      <Paper
        elevation={2}
        style={{
          display: "flex",
          flexDirection: "column",
          alignContent: "space-between",
          padding: "0px",
        }}
      >
        <LoadingIndicator isLoading={isLoading} />
        <List
          disablePadding
          style={{
            width: "100%",
            minHeight: "68vh",
            maxHeight: "68vh",
            overflowX: "scroll",
          }}
        >
          {filteredData.map((item) => (
            <SpamMailClassificationItem
              key={item.id}
              item={item}
              handleModifiedItem={handleModifiedItem}
            />
          ))}
        </List>

        <StyledBox width="100%" height="20%" marginTop="2rem">
          <StyledBox
            display="flex"
            justifyContent="flex-end"
            alignItems="baseline"
            gap={2}
            padding={2}
          >
            <FormButton
              label={getResource("common.labelCancel")}
              disabled={!isModified}
              onAction={handleCancel}
            />
            <FormButton
              label={getResource("common.labelSave")}
              disabled={!isModified}
              onAction={onSave}
            />
          </StyledBox>
        </StyledBox>
      </Paper>
    </FilterLayout>
  );
};

export default SpamMailClassificationPage;
