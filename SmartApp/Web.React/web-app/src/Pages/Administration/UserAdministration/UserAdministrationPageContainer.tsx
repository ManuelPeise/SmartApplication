import { Box } from "@mui/material";
import React from "react";
import { useAuth } from "src/_hooks/useAuth";
import { useComponentInitialization } from "src/_hooks/useComponentInitialization";
import { UserAdministrationPageProps } from "./Types/UserAdministrationPageProps";
import { StatelessApi } from "src/_lib/_api/StatelessApi";
import { UserAdministrationUserModel } from "./Types/UserAdministrationUserModel";
import SettingsLayout, {
  SettingsListItem,
} from "src/_components/Layouts/SettingsLayout";
import { useI18n } from "src/_hooks/useI18n";
import UserDataDetails from "./Components/UserDataDetails";

const initializeAsync = async (
  token: string
): Promise<UserAdministrationPageProps> => {
  const api = StatelessApi.create();

  const onLoadUsers = async () => {
    return await api.get<UserAdministrationUserModel[]>(
      { serviceUrl: "UserAdministration/LoadUsers" },
      token
    );
  };

  const onUpdateUser = async (model: UserAdministrationUserModel) => {
    return await api.post<boolean>(
      { serviceUrl: "UserAdministration/UpdateUser", body: model },
      token
    );
  };

  const [users] = await Promise.all([onLoadUsers()]);

  return {
    users: users,
    onUpdateUser: onUpdateUser,
  };
};

const UserAdministrationPageContainer: React.FC = () => {
  const { authenticationState } = useAuth();
  const { getResource } = useI18n();

  const [selectedUserId, setSelectedUserId] = React.useState<number>(1);
  const [tempUserId, setTempUserId] = React.useState<number>(1);

  const { initProps } = useComponentInitialization<UserAdministrationPageProps>(
    authenticationState.token,
    initializeAsync
  );

  const [users, setUsers] = React.useState<UserAdministrationUserModel[]>(
    initProps?.users
  );

  const onSelectedUserChanged = React.useCallback((state: number) => {
    setSelectedUserId(state);
  }, []);

  React.useEffect(() => {
    if (initProps?.users) {
      setUsers(initProps.users);
    }
  }, [initProps]);

  const userListItems = React.useMemo((): SettingsListItem[] => {
    return (
      users
        ?.sort((a, b) => (a.userId > b.userId ? 1 : -1))
        .map((user) => {
          return {
            id: user.userId,
            label: `${user.firstName} ${user.lastName}`,
            description: getResource("administration.labelDetails"),
            selected: selectedUserId === user.userId,
            readonly: selectedUserId === user.userId,
            onSectionChanged: onSelectedUserChanged,
            onMouseOver: (id) => {
              setTempUserId(id);
            },
          };
        }) ?? []
    );
  }, [selectedUserId, users, onSelectedUserChanged, getResource]);

  const handleUpdateUsers = React.useCallback(
    (model: UserAdministrationUserModel) => {
      const copy = users.slice();

      const index = copy.findIndex((x) => x.userId === model.userId) ?? null;

      if (index !== null) {
        copy[index] = model;

        setUsers(copy);
      }
    },
    [users]
  );

  if (initProps?.users == null) {
    return null;
  }

  return (
    <Box width="100%" height="100%">
      <Box padding={4}>
        <SettingsLayout listItems={userListItems} selectedItem={selectedUserId}>
          <Box height="100%">
            {users?.map((user, key) => (
              <UserDataDetails
                key={key}
                selectedUserId={selectedUserId}
                tempUserId={tempUserId}
                user={user}
                onResetTempUserId={setTempUserId}
                onUpdateUser={initProps.onUpdateUser}
                onUpdateUsersState={handleUpdateUsers}
                onSelectedUserChanged={onSelectedUserChanged}
              />
            ))}
          </Box>
        </SettingsLayout>
      </Box>
    </Box>
  );
};

export default UserAdministrationPageContainer;
