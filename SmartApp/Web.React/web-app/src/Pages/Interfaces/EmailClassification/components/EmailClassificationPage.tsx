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

interface IProps {
  classifications: EmailClassificationModel[];
  folders: EmailFolderModel[];
}

const EmailClassificationPage: React.FC<IProps> = (props) => {
  const { classifications, folders } = props;
  const { getResource } = useI18n();

  const [filter, setFilter] = React.useState<EmailFilter>({
    address: "",
    hideHam: false,
    hideSpam: false,
  });

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
      return [
        {
          name: "emailAddress",
          headerLabel: getResource("interface.labelEmailAddress"),
          percentageWidth: 0.1,
          align: "left",
          width: 400,
          hasToolTip: false,
          component: emailTableCell,
        },
        // {
        //   name: "domain",
        //   headerLabel: getResource("interface.labelDomain"),
        //   percentageWidth: 0.2,
        //   align: "left",
        //   width: 300,
        //   hasToolTip: false,
        //   component: emailTableCell,
        // },
        {
          name: "subject",
          headerLabel: getResource("interface.labelSubject"),
          percentageWidth: 0.3,
          align: "left",
          width: 600,
          hasToolTip: true,
          component: emailTableCell,
        },
        {
          name: "isSpam",
          headerLabel: getResource("interface.labelIsSpam"),
          percentageWidth: 0.3,
          align: "center",
          width: 200,
          hasToolTip: true,
          component: emailTableCell,
        },
        {
          name: "predictedAsSpam",
          headerLabel: getResource("interface.labelSpamPrediction"),
          percentageWidth: 0.3,
          align: "center",
          width: 200,
          hasToolTip: false,
          isReadonly: true,
          component: emailTableCell,
        },
        {
          name: "targetFolderId",
          headerLabel: getResource("interface.labelTargetFolder"),
          percentageWidth: 0.3,
          align: "left",
          width: 200,
          hasToolTip: false,
          component: emailTableCell,
        },
        {
          name: "predictedTargetFolderId",
          headerLabel: getResource("interface.labelPredictedTargetFolder"),
          percentageWidth: 0.3,
          align: "left",
          width: 200,
          hasToolTip: false,
          component: emailTableCell,
        },
      ];
    }, [getResource]);

  const saveCancelButtonProps = React.useMemo((): ButtonProps[] => {
    return [
      {
        label: "save",
        onAction: () => {},
      },
    ];
  }, []);

  const datasets = React.useMemo((): EmailClassificationModel[] => {
    let models = [...classifications];

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
  }, [classifications, filter]);

  return (
    <Grid2 container alignContent="flex-start" width="100%" gap={2} padding={2}>
      <EmailFilterToolBar
        itemsCount={datasets.length}
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
          />
        </DetailsView>
      </Paper>
    </Grid2>
  );
};

export default EmailClassificationPage;
