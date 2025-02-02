import React, { PropsWithChildren } from "react";
import { useAuth } from "src/_hooks/useAuth";
import { StatelessApi } from "src/_lib/_api/StatelessApi";
import {
  AccessRightContextProps,
  AccessRightValues,
  UserAccessRightModel,
} from "src/_lib/_types/auth";

export const AccessRightContext = React.createContext<AccessRightContextProps>(
  {} as AccessRightContextProps
);

const AccessRightsContextProvider: React.FC<PropsWithChildren> = (props) => {
  const { children } = props;
  const { authenticationState } = useAuth();

  const [accessRights, setAccessRights] =
    React.useState<UserAccessRightModel | null>(null);
  const api = StatelessApi.create();

  const loadAccessRights = React.useCallback(async () => {
    await api
      .get<UserAccessRightModel>(
        {
          serviceUrl: "UserAccessRight/LoadUserAccessRights",
          parameters: { userId: `${authenticationState.jwtData.userId}` },
        },
        authenticationState.token
      )
      .then((response) => {
        setAccessRights(response);
      });
  }, [api, authenticationState.jwtData.userId, authenticationState.token]);

  const userHasAccess = React.useCallback(
    (name: string, value: keyof AccessRightValues): boolean => {
      const rights = accessRights.accessRights ?? [];

      const selectedRight = rights.find((x) => x.name === name) ?? null;

      if (selectedRight == null) {
        return false;
      }

      return selectedRight[value];
    },
    [accessRights?.accessRights]
  );

  const getAccessRight = React.useCallback(
    (name: string) => {
      const rights = accessRights?.accessRights ?? [];

      const selectedRight = rights.find((x) => x.name === name) ?? null;

      if (!selectedRight) {
        return { deny: true, canView: false, canEdit: false };
      }

      return {
        deny: selectedRight.deny,
        canView: selectedRight.canView,
        canEdit: selectedRight.canEdit,
      };
    },
    [accessRights?.accessRights]
  );

  React.useEffect(() => {
    if (authenticationState && authenticationState.token) {
      loadAccessRights();
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [authenticationState]);

  return (
    <AccessRightContext.Provider
      value={{
        accessRights: accessRights,
        userHasAssess: userHasAccess,
        getAccessRight: getAccessRight,
      }}
    >
      {children}
    </AccessRightContext.Provider>
  );
};

export default AccessRightsContextProvider;
