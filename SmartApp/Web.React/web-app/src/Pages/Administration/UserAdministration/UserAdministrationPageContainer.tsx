import React from "react";
import { useAuth } from "src/_hooks/useAuth";
import { StatelessApi } from "src/_lib/_api/StatelessApi";
import { UserAdministrationUserModel } from "./Types/UserAdministrationUserModel";
import UserAdministrationPage from "./UserAdministrationPage";
import { useAccessRights } from "src/_hooks/useAccessRights";
import { UserRightTypeEnum } from "src/_lib/Router/routeTypes";
import { useStatefulApiService } from "src/_hooks/useStatefulApiService";

const UserAdministrationPageContainer: React.FC = () => {
  const { authenticationState } = useAuth();

  const { accessRights } = useAccessRights();

  const api = StatelessApi.create();

  const { isLoading, data, sendPost, rebindData } = useStatefulApiService<
    UserAdministrationUserModel[]
  >(
    api,
    { serviceUrl: "UserAdministration/LoadUsers" },
    authenticationState.token
  );

  const requiredRight = React.useMemo(() => {
    return accessRights.accessRights.find(
      (x) => x.name === UserRightTypeEnum.UserAdministration
    );
  }, [accessRights]);

  if (data == null) {
    return null;
  }

  return (
    <UserAdministrationPage
      users={data}
      accessRight={requiredRight}
      isLoading={isLoading}
      sendPost={sendPost}
      onRebind={rebindData}
    />
  );
};

export default UserAdministrationPageContainer;
