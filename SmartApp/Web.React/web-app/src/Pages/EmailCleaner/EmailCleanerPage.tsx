import { Grid2 } from "@mui/material";
import React from "react";
import { useAuth } from "src/_hooks/useAuth";
import { StatelessApi } from "src/_lib/_api/StatelessApi";
import EmailCleanerHeader from "./Components/EmailCleanerHeader";
import {
  ConnectionTestModel,
  EmailAccountModel,
  EmailAddressMappingEntry,
  EmailCleanerOverlayStatus,
  EmailCleanerSettings,
  FolderSettings,
} from "./EmailCleanerTypes";
import EmailCleanerOverlay from "./Components/EmailCleanerOverlay";
import AddEmailAccountForm from "./Components/AddEmailAccountForm";
import { useStatefulApiService } from "src/_hooks/useStatefulApiService";
import { extractValues } from "src/_lib/utils";
import EmailCleanerBody from "./Components/EmailCleanerBody";

const EmailCleanerPageContainer: React.FC = () => {
  const { authenticationState } = useAuth();
  const api = StatelessApi.create();
  const { isLoading, data, sendPost, sendGet, rebindData } =
    useStatefulApiService<EmailAccountModel[]>(
      api,
      { serviceUrl: "EmailCleanerAccount/GetEmailAccounts" },
      authenticationState.token
    );
  const [overlayState, setOverlysState] =
    React.useState<EmailCleanerOverlayStatus>({ addAccountOverlayOpen: false });
  const [selectedAccount, setSelectedAccount] = React.useState<number>(-1);

  const handleTestConnection = React.useCallback(
    async (model: ConnectionTestModel): Promise<boolean> => {
      return await sendPost<boolean>({
        serviceUrl: "EmailCleanerAccount/TestAccountConnection",
        body: model,
      });
    },
    [sendPost]
  );

  const handleSaveConnection = React.useCallback(
    async (model: EmailAccountModel): Promise<boolean> => {
      return await sendPost<boolean>({
        serviceUrl: "EmailCleanerAccount/SaveAccount",
        body: model,
      }).then(async (res) => {
        if (res) {
          await rebindData();
        }
        return res;
      });
    },
    [sendPost, rebindData]
  );

  const handleGetUpdatedFolders = React.useCallback(
    async (accountId: number): Promise<FolderSettings[]> => {
      return await sendGet<FolderSettings[]>({
        serviceUrl: "EmailCleanerAccount/GetFolderUpdate",
        parameters: { accountId: accountId.toString() },
      });
    },
    [sendGet]
  );

  const handleUpdateSettings = React.useCallback(
    async (model: EmailCleanerSettings): Promise<void> => {
      await sendPost<boolean>({
        serviceUrl: "EmailCleanerAccount/UpdateSettings",
        body: model,
      }).then(async (res) => {
        if (res) {
          await rebindData();
        }
      });
    },
    [sendPost, rebindData]
  );

  const handleUpdateAccount = React.useCallback(
    async (model: EmailAccountModel): Promise<void> => {
      await sendPost<boolean>({
        serviceUrl: "EmailCleanerAccount/UpdateAccount",
        body: model,
      }).then(async (res) => {
        if (res) {
          await rebindData();
        }
      });
    },
    [sendPost, rebindData]
  );

  const handleOverlayStateChanged = React.useCallback(
    (partialOverlayState: Partial<EmailCleanerOverlayStatus>) => {
      setOverlysState({ ...overlayState, ...partialOverlayState });
    },
    [overlayState]
  );

  const handleSelectedAccountChanged = React.useCallback((index: number) => {
    setSelectedAccount(index);
  }, []);

  const handleUpdateEmailAddressMappings = React.useCallback(
    async (mappings: EmailAddressMappingEntry[]) => {
      await sendPost<boolean>({
        serviceUrl: "EmailCleanerAccount/UpdateEmailAddressMappingEntries",
        body: JSON.stringify(mappings),
      }).then(async () => {
        await rebindData();
      });
    },
    [sendPost, rebindData]
  );

  const accountNames = React.useMemo((): string[] => {
    return extractValues<string, EmailAccountModel>(data ?? [], "accountName");
  }, [data]);

  return (
    <Grid2 id="email-cleaner-page-container" minWidth="100%" container p={1}>
      <Grid2 container width="100%" gap={2}>
        <EmailCleanerHeader
          accountNames={accountNames}
          selectedAccountIndex={selectedAccount}
          overlayState={overlayState}
          handleOverlayStateChanged={handleOverlayStateChanged}
          handleSelectedAccountChanged={handleSelectedAccountChanged}
        />
        <EmailCleanerBody
          isLoading={isLoading}
          selectedAccount={data && data[selectedAccount]}
          handleGetUpdatedFolders={handleGetUpdatedFolders}
          handleTestConnection={handleTestConnection}
          handleSaveConnection={handleSaveConnection}
          handleUpdateSettings={handleUpdateSettings}
          handleUpdateAccount={handleUpdateAccount}
          handleUpdateEmailAddressMappings={handleUpdateEmailAddressMappings}
        />
        <EmailCleanerOverlay open={overlayState.addAccountOverlayOpen}>
          <AddEmailAccountForm
            userId={authenticationState.jwtData.userId}
            isLoading={isLoading}
            handleTestConnection={handleTestConnection}
            handleClose={handleOverlayStateChanged.bind(null, {
              addAccountOverlayOpen: false,
            })}
            handleSaveConnection={handleSaveConnection}
          />
        </EmailCleanerOverlay>
      </Grid2>
    </Grid2>
  );
};
export default EmailCleanerPageContainer;
