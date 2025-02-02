import React from "react";
import {
  EmailClassificationModel,
  EmailFilter,
  EmailFolderModel,
  EmailTableColumn,
} from "../types";
import { Grid2, Paper } from "@mui/material";
import DetailsView, { ButtonProps } from "src/_components/Layouts/DetailsView";
import EmailClassificationTable from "./EmailClassificationTable";
import { useI18n } from "src/_hooks/useI18n";
import { emailTableCell } from "./EmailTableColumns";
import { DropDownItem } from "src/_components/Input/Dropdown";
import EmailFilterToolBar from "./EmailFilterToolBar";
import { isEqual } from "lodash";

interface IProps {
  classifications: EmailClassificationModel[];
  folders: EmailFolderModel[];
  spamPredictionEnabled: boolean;
  folderPredictionEnabled: boolean;
  handleSave: (items: EmailClassificationModel[]) => Promise<void>;
}

const EmailClassificationPage: React.FC<IProps> = (props) => {
  const {
    classifications,
    folders,
    folderPredictionEnabled,
    spamPredictionEnabled,
    handleSave,
  } = props;
  const { getResource } = useI18n();
  const [filter, setFilter] = React.useState<EmailFilter>({
    address: "",
    hideHam: false,
    hideSpam: false,
    modifyEntireFilterResult: false,
  });

  const [intermediateState, setIntermediateState] =
    React.useState<EmailClassificationModel[]>(classifications);

  const handleItemsChanged = React.useCallback(
    (partialState: Partial<EmailClassificationModel>, id: number) => {
      console.log("Changed", id);

      if (filter.modifyEntireFilterResult) {
        const sourceAddress = intermediateState.find(
          (x) => x.id === id
        ).emailAddress;

        const update = intermediateState.map((e) =>
          e.emailAddress === sourceAddress ? { ...e, ...partialState } : e
        );

        setIntermediateState(update);
      } else {
        const update = intermediateState.map((e) =>
          e.id === id ? { ...e, ...partialState } : e
        );

        setIntermediateState(update);
      }
    },
    [filter.modifyEntireFilterResult, intermediateState]
  );

  const handleFilterChanged = React.useCallback(
    (partialFilter: Partial<EmailFilter>) => {
      setFilter({ ...filter, ...partialFilter });
    },
    [filter]
  );

  const folderDropdownItems = React.useMemo((): DropDownItem[] => {
    const folderItems = folders?.map((f) => {
      return {
        key: f.folderId,
        label: getResource(`interface.${f.resourceKey}`),
      };
    });

    return folderItems.sort((a, b) => (a.label < b.label ? -1 : 1));
  }, [folders, getResource]);

  const tableColumnDefinitions =
    React.useMemo((): EmailTableColumn<EmailClassificationModel>[] => {
      const columns: EmailTableColumn<EmailClassificationModel>[] = [];

      columns.push({
        name: "emailAddress",
        headerLabel: getResource("interface.labelEmailAddress"),
        align: "left",
        width: 400,
        hasToolTip: true,
        component: emailTableCell,
      });

      columns.push({
        name: "subject",
        headerLabel: getResource("interface.labelSubject"),
        align: "left",
        width:
          spamPredictionEnabled && folderPredictionEnabled
            ? 500
            : spamPredictionEnabled || folderPredictionEnabled
            ? 600
            : 700,
        hasToolTip: true,
        component: emailTableCell,
      });

      columns.push({
        name: "isSpam",
        headerLabel: getResource("interface.labelIsSpam"),
        align: "center",
        width: 160,
        hasToolTip: true,
        component: emailTableCell,
      });

      if (spamPredictionEnabled) {
        columns.push({
          name: "predictedAsSpam",
          headerLabel: getResource("interface.labelPrediction"),
          align: "center",
          width: 100,
          hasToolTip: false,
          isReadonly: true,
          component: emailTableCell,
        });
      }

      columns.push({
        name: "backup",
        headerLabel: getResource("interface.labelBackup"),
        align: "center",
        width: 100,
        hasToolTip: false,
        isReadonly: false,
        component: emailTableCell,
      });

      columns.push({
        name: "delete",
        headerLabel: getResource("interface.labelDelete"),
        align: "center",
        width: 100,
        hasToolTip: false,
        isReadonly: false,
        component: emailTableCell,
      });

      columns.push({
        name: "targetFolderId",
        headerLabel: getResource("interface.labelTargetFolder"),
        align: "left",
        width: 200,
        hasToolTip: false,
        component: emailTableCell,
      });

      if (folderPredictionEnabled) {
        columns.push({
          name: "predictedTargetFolderId",
          headerLabel: getResource("interface.labelPrediction"),
          align: "center",
          width: 100,
          hasToolTip: false,
          component: emailTableCell,
        });
      }

      columns.push({
        name: "update",
        headerLabel: getResource("interface.labelUpdate"),
        align: "center",
        width: 100,
        isReadonly: !spamPredictionEnabled && !folderPredictionEnabled,
        hasToolTip: false,
        component: emailTableCell,
      });

      return columns;
    }, [folderPredictionEnabled, getResource, spamPredictionEnabled]);

  const modifiedIds = React.useMemo((): number[] => {
    const ids: number[] = [];
    classifications.forEach((c) => {
      if (
        !isEqual(
          c,
          intermediateState.find((x) => x.id === c.id)
        )
      ) {
        ids.push(c.id);
      }
    });

    return ids;
  }, [classifications, intermediateState]);

  const saveCancelButtonProps = React.useMemo((): ButtonProps[] => {
    return [
      {
        label: getResource("common.labelCancel"),
        disabled: !modifiedIds.length,
        onAction: () => setIntermediateState(classifications),
      },
      {
        label: getResource("common.labelSave"),
        disabled: !modifiedIds.length,
        onAction: handleSave.bind(
          null,
          intermediateState.filter((x) => modifiedIds.includes(x.id))
        ),
      },
    ];
  }, [
    getResource,
    modifiedIds,
    handleSave,
    intermediateState,
    classifications,
  ]);

  const datasets = React.useMemo((): EmailClassificationModel[] => {
    let models = [...intermediateState];

    if (filter.hideHam) {
      models = models.filter((x) => x.isSpam);
    }

    if (filter.hideSpam) {
      models = models.filter((x) => !x.isSpam);
    }

    if (filter.address.length) {
      models = models.filter((x) =>
        x.emailAddress.toLowerCase().startsWith(filter.address.toLowerCase())
      );
    }

    return models;
  }, [intermediateState, filter]);

  const addressItems = React.useMemo((): string[] => {
    const items: string[] = [];

    classifications.forEach((set, index) => {
      if (!items.find((x) => x === set.emailAddress)) {
        items.push(set.emailAddress);
      }
    });

    return items;
  }, [classifications]);

  // ensure 'modifyEntireFilterResult' is set to false if address filter is cleared
  React.useEffect(() => {
    if (filter.address === "") {
      handleFilterChanged({ modifyEntireFilterResult: false });
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [filter.address]);

  // ensure the state is updated if new state comes from backend
  React.useEffect(() => {
    setIntermediateState(classifications);
  }, [classifications]);

  return (
    <Grid2 container alignContent="flex-start" width="100%" gap={2} padding={2}>
      <EmailFilterToolBar
        itemsCount={datasets.length}
        addressItems={addressItems}
        filter={filter}
        handleFilterChanged={handleFilterChanged}
      />
      <Paper
        sx={{ display: "flex", height: "auto", width: "100%", padding: 2 }}
      >
        <DetailsView
          saveCancelButtonProps={saveCancelButtonProps}
          justifyContent="center"
        >
          <EmailClassificationTable
            classifications={datasets}
            columnDefinitions={tableColumnDefinitions}
            folderDropdownItems={folderDropdownItems}
            handleItemsChanged={handleItemsChanged}
          />
        </DetailsView>
      </Paper>
    </Grid2>
  );
};

export default EmailClassificationPage;
