import {
  Grid2,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Typography,
} from "@mui/material";
import React from "react";
import {
  EmailDomainModel,
  SaveTrainingDataRequest,
  SpamClassificationColumn,
  SpamClassificationDataSet,
} from "./types";
import { useI18n } from "src/_hooks/useI18n";
import Dropdown from "src/_components/Input/Dropdown";
import DetailsView, { ButtonProps } from "src/_components/Layouts/DetailsView";
import EmailClassificationFilter from "./EmailClassificationFilter";
import NoDomainSelectedPlaceholder from "./NoDomainSelectedPlaceholder";

interface IProps {
  tabIndex: number;
  selectedTab: number;
  domains: EmailDomainModel[];
  handleModifiedState: React.Dispatch<React.SetStateAction<boolean>>;
  saveTrainingData: (request: SaveTrainingDataRequest) => Promise<boolean>;
}

const EmailClassificationDefinitionTab: React.FC<IProps> = (props) => {
  const {
    tabIndex,
    selectedTab,
    domains,
    handleModifiedState,
    saveTrainingData,
  } = props;
  const { getResource } = useI18n();

  const [modifiedClassificationIds, setModifiedClassificationIds] =
    React.useState<number[]>([]);
  const [domainId, setDomainId] = React.useState<number>(-1);
  const [domainData, setDomainData] = React.useState<EmailDomainModel | null>(
    null
  );

  const handleDomainChanged = React.useCallback(
    (domainId: number) => {
      setDomainId(domainId);
      setDomainData(domains.find((x) => x.id === domainId));
    },
    [domains]
  );

  const handleRowChanged = React.useCallback(
    (id: number, value: number) => {
      setDomainData({
        ...domainData,
        classificationDataSets: [
          ...domainData.classificationDataSets.map((data) =>
            data.id === id ? { ...data, classification: value } : data
          ),
        ],
      });

      if (modifiedClassificationIds.includes(id)) {
        setModifiedClassificationIds(
          modifiedClassificationIds.filter((x) => x !== id)
        );
      } else {
        const ids = [...modifiedClassificationIds];
        ids.push(id);

        setModifiedClassificationIds(ids);
      }
    },
    [domainData, modifiedClassificationIds]
  );

  const columnDefinitions = React.useMemo((): SpamClassificationColumn[] => {
    return [
      {
        key: "email",
        label: getResource("administration.labelEmailAddress"),
        minWidth: 200,
        align: "left",
        renderComponent: (props: SpamClassificationDataSet) => {
          return <Typography variant="body1">{props.email}</Typography>;
        },
      },
      {
        key: "subject",
        label: getResource("administration.labelEmailSubject"),
        minWidth: 200,
        align: "left",
        renderComponent: (props: SpamClassificationDataSet) => {
          return <Typography variant="body1">{props.subject}</Typography>;
        },
      },
      {
        key: "classification",
        label: getResource("administration.labelEmailClassification"),
        minWidth: 200,
        align: "left",
        renderComponent: (props: SpamClassificationDataSet) => {
          return (
            <Dropdown
              value={props.classification}
              minWidth={200}
              items={[
                {
                  key: 0,
                  label: getResource("administration.labelUnknown"),
                  disabled: props.classification === 0,
                },
                {
                  key: 1,
                  label: getResource("administration.labelHam"),
                  disabled: props.classification === 1,
                },
                {
                  key: 2,
                  label: getResource("administration.labelSpam"),
                  disabled: props.classification === 2,
                },
              ]}
              onChange={(value) => handleRowChanged(props.id, value)}
              disabled={false}
            />
          );
        },
      },
    ];
  }, [getResource, handleRowChanged]);

  const isModified = React.useMemo(() => {
    return modifiedClassificationIds?.length;
  }, [modifiedClassificationIds]);

  const onSaveTrainingData = React.useCallback(async () => {
    if (!modifiedClassificationIds.length) return;
    const result = await saveTrainingData({
      models: domainData.classificationDataSets.filter((x) =>
        modifiedClassificationIds.includes(x.id)
      ),
    });

    if (result) setModifiedClassificationIds([]);
  }, [
    domainData?.classificationDataSets,
    modifiedClassificationIds,
    saveTrainingData,
  ]);

  const saveCancelButtonProps = React.useMemo((): ButtonProps[] => {
    return [
      {
        disabled: !isModified,
        label: getResource("common.labelCancel"),
        onAction: () => setDomainData(domains.find((x) => x.id === domainId)),
      },
      {
        disabled: !isModified,
        label: getResource("common.labelSave"),
        onAction: onSaveTrainingData,
      },
    ];
  }, [domainId, domains, getResource, isModified, onSaveTrainingData]);

  React.useEffect(() => {
    if (modifiedClassificationIds.length) {
      handleModifiedState(true);
      return;
    }

    handleModifiedState(false);
  }, [handleModifiedState, modifiedClassificationIds.length]);

  React.useEffect(() => {
    if (tabIndex !== selectedTab) setDomainData(null);
  }, [tabIndex, selectedTab]);

  if (tabIndex !== selectedTab || !domains.length) {
    return null;
  }

  return (
    <DetailsView
      saveCancelButtonProps={saveCancelButtonProps}
      additionalButtonProps={[]}
    >
      <Grid2 width="100%" display="flex" justifyContent="center">
        <Grid2 width="98%">
          <EmailClassificationFilter
            data={domains}
            selectedDomainId={domainId}
            handleChange={handleDomainChanged}
          />
          {domainData ? (
            <TableContainer sx={{ maxHeight: 600 }}>
              <Table stickyHeader>
                <TableHead>
                  <TableRow>
                    {columnDefinitions.map((coldef, index) => (
                      <TableCell
                        key={`table-header-${index}`}
                        align={coldef.align}
                        sx={{ minWidth: coldef.minWidth }}
                      >
                        <Typography>{coldef.label}</Typography>
                      </TableCell>
                    ))}
                  </TableRow>
                </TableHead>
                <TableBody>
                  {domainData?.classificationDataSets?.map((row, rowIndex) => (
                    <TableRow key={rowIndex} hover>
                      {columnDefinitions.map((coldef, colIndex) => (
                        <TableCell
                          key={`table-body-cell-${row.id}-${colIndex}`}
                          align={coldef.align}
                          sx={{ minWidth: coldef.minWidth }}
                        >
                          {coldef.renderComponent(row)}
                        </TableCell>
                      ))}
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </TableContainer>
          ) : (
            <NoDomainSelectedPlaceholder maxHeight={600} />
          )}
        </Grid2>
      </Grid2>
    </DetailsView>
  );
};

export default EmailClassificationDefinitionTab;
