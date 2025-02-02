import React from "react";
import { useParams } from "react-router-dom";
import { useAuth } from "src/_hooks/useAuth";
import { useStatefulApiService } from "src/_hooks/useStatefulApiService";
import { StatelessApi } from "src/_lib/_api/StatelessApi";
import { EmailClassificationPageModel } from "./types";
import EmailClassificationPage from "./components/EmailClassificationPage";

const EmailClassificationContainer: React.FC = () => {
  const { authenticationState } = useAuth();
  const { id } = useParams();

  const api = StatelessApi.create();

  const { data } = useStatefulApiService<EmailClassificationPageModel>(
    api,
    {
      serviceUrl: "EmailClassification/GetSpamClassificationData",
      parameters: { accountId: id },
    },
    authenticationState.token
  );

  if (data == null || !data?.classificationModels?.length) {
    return null;
  }

  return (
    <EmailClassificationPage
      classifications={data.classificationModels}
      folders={data.folders}
    />
  );
};

export default EmailClassificationContainer;
