import { ModelTrainingRounded } from "@mui/icons-material";
import {
  Box,
  Card,
  CardContent,
  Grid2,
  IconButton,
  Paper,
  Tooltip,
  Typography,
} from "@mui/material";
import React from "react";
import DetailsView from "src/_components/Layouts/DetailsView";
import {
  EmailPrediction,
  PredictionRequest,
  SpamPredictionStatisticData,
} from "./types";
import { useI18n } from "src/_hooks/useI18n";
import { colors } from "src/_lib/colors";
import TextInput from "src/_components/Input/TextInput";
import FormButton from "src/_components/Buttons/FormButton";
import { emailValidation } from "src/_lib/validation";

interface IProps {
  tabIndex: number;
  selectedTab: number;
  statistics: SpamPredictionStatisticData;
  handleTrainModel: () => Promise<void>;
  handlePredictEmail: (model: PredictionRequest) => Promise<EmailPrediction>;
}

const EmailClassificationInfoTab: React.FC<IProps> = (props) => {
  const {
    tabIndex,
    selectedTab,
    statistics,
    handleTrainModel,
    handlePredictEmail,
  } = props;
  const { getResource } = useI18n();

  const [prediction, setPrediction] = React.useState<EmailPrediction>({
    request: {
      emailAddress: "",
      subject: "",
    },
    probability: 0,
    score: 0,
  });

  const handlePredictionChanged = React.useCallback(
    (partialPrediction: Partial<EmailPrediction>) => {
      setPrediction({ ...prediction, ...partialPrediction });
    },
    [prediction]
  );

  const onPredict = React.useCallback(async () => {
    const result = await handlePredictEmail(prediction.request);

    if (result) {
      handlePredictionChanged({
        label: result.label,
        score: result.score,
        probability: result.probability,
        request: { emailAddress: "", subject: "" },
      });
    }
  }, [handlePredictEmail, handlePredictionChanged, prediction.request]);

  const canPredict = React.useMemo(() => {
    return (
      prediction.request.emailAddress.length &&
      emailValidation(prediction.request.emailAddress) &&
      prediction.request.subject.length
    );
  }, [prediction]);

  if (tabIndex !== selectedTab) {
    return null;
  }

  return (
    <DetailsView saveCancelButtonProps={[]} additionalButtonProps={[]}>
      <Grid2
        width="100%"
        display="flex"
        justifyContent="center"
        flexWrap="wrap"
        flexDirection="row"
        alignContent="baseline"
        padding={2}
        gap={2}
      >
        <Grid2 size={12} sx={{ minWidth: "400px" }}>
          <Card sx={{ minHeight: 200, padding: 2 }}>
            <CardContent sx={{ display: "flex", flexDirection: "column" }}>
              <Box display="flex" flexDirection="row">
                <Box
                  width="100%"
                  display="flex"
                  flexDirection="row"
                  justifyContent="space-between"
                >
                  <Box
                    display="flex"
                    justifyContent="center"
                    flexDirection="column"
                  >
                    <Typography
                      sx={{
                        textAlign: "center",
                        fontSize: "2rem",
                        color: colors.typography.blue,
                      }}
                    >
                      {statistics.averageEntrophy.toFixed(2)}
                    </Typography>
                    <Typography
                      sx={{
                        fontSize: "1.2rem",
                        textAlign: "center",
                        color: colors.typography.lightgray,
                      }}
                    >
                      {getResource("administration.labelAverageEntrophy")}
                    </Typography>
                  </Box>
                  <Box>
                    <Tooltip
                      title={getResource("administration.labelTrainModel")}
                      children={
                        <IconButton size="small" onClick={handleTrainModel}>
                          <ModelTrainingRounded
                            sx={{
                              color: "#cccccc",
                              height: 50,
                              width: 50,
                              "&:hover": {
                                color: colors.darkgray,
                                cursor: "pointer",
                              },
                            }}
                          />
                        </IconButton>
                      }
                    />
                  </Box>
                </Box>
              </Box>
              <Box display="flex" flexDirection="column">
                <Box
                  paddingTop={2}
                  display="flex"
                  justifyContent="space-between"
                  alignItems="baseline"
                >
                  <Typography
                    sx={{
                      whiteSpace: "break-spaces",
                      fontSize: "1.2rem",
                      color: colors.typography.darkgray,
                    }}
                  >
                    {getResource(
                      "administration.labelAvailableTrainingDataSets"
                    )}
                  </Typography>
                  <Typography
                    sx={{ whiteSpace: "break-spaces", fontSize: "1rem" }}
                  >
                    {statistics?.trainingEntityCount ?? "n/a"}
                  </Typography>
                </Box>
                <Box
                  display="flex"
                  justifyContent="space-between"
                  alignItems="baseline"
                  paddingTop={2}
                >
                  <Typography
                    sx={{
                      whiteSpace: "break-spaces",
                      fontSize: "1.2rem",
                      color: colors.typography.darkgray,
                    }}
                  >
                    {getResource("administration.labelLastUpdateAt").replace(
                      "{AT}",
                      `${statistics?.modelsFileTimeStamp ?? "n/a"}`
                    )}
                  </Typography>
                  <Typography
                    sx={{ whiteSpace: "break-spaces", fontSize: "1rem" }}
                  >
                    {statistics?.modelsFileTimeStamp ?? "n/a"}
                  </Typography>
                </Box>
              </Box>
            </CardContent>
          </Card>
        </Grid2>
        <Grid2 size={12} sx={{ minWidth: "400px" }}>
          <Paper sx={{ height: "inherit", padding: 2 }}>
            <Grid2
              display="flex"
              justifyContent="flex-start"
              flexDirection="column"
              p={2}
              gap={2}
            >
              <Grid2 size={12} padding={2}>
                <Typography
                  sx={{
                    fontSize: "1.5rem",
                    fontWeight: "500",
                    color: colors.typography.blue,
                  }}
                >
                  {getResource("administration.labelAiPredictionTest")}
                </Typography>
              </Grid2>
              <Grid2
                size={12}
                padding={1}
                display="flex"
                flexDirection="row"
                justifyContent="space-around"
                borderRadius={8}
                border={`1px solid ${colors.lightgray}`}
              >
                <Typography>
                  {getResource("administration.labelPredictedValue").replace(
                    "{Label}",
                    prediction?.label === undefined
                      ? "n/a"
                      : prediction.label
                      ? "Spam"
                      : "Ham"
                  )}
                </Typography>
                <Typography>
                  {getResource("administration.labelPredictedScore").replace(
                    "{Score}",
                    prediction.score.toFixed(2)
                  )}
                </Typography>
                <Typography>
                  {getResource(
                    "administration.labelPredictedProbability"
                  ).replace(
                    "{Probability}",
                    (prediction.probability * 100).toFixed(2)
                  )}
                </Typography>
              </Grid2>
              {/* form */}
              <Grid2
                size={12}
                display="flex"
                justifyContent="flex-start"
                flexDirection="column"
                p={2}
                gap={2}
              >
                <Grid2 size={12}>
                  <TextInput
                    fullwidth
                    label={getResource("administration.labelEmail")}
                    value={prediction.request.emailAddress}
                    onChange={(value) =>
                      handlePredictionChanged({
                        request: { ...prediction.request, emailAddress: value },
                      })
                    }
                  />
                </Grid2>
                <Grid2 size={12}>
                  <TextInput
                    fullwidth
                    label={getResource("administration.labelEmailSubject")}
                    value={prediction.request.subject}
                    onChange={(value) =>
                      handlePredictionChanged({
                        request: { ...prediction.request, subject: value },
                      })
                    }
                  />
                </Grid2>
                {/* button */}
                <Grid2
                  size={12}
                  display="flex"
                  justifyContent="flex-end"
                  paddingTop={2}
                >
                  <FormButton
                    label={getResource("administration.labelPredict")}
                    disabled={!canPredict}
                    onAction={onPredict}
                  />
                </Grid2>
              </Grid2>
            </Grid2>
          </Paper>
        </Grid2>
      </Grid2>
    </DetailsView>
  );
};

export default EmailClassificationInfoTab;
