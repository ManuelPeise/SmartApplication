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

  const [users] = await Promise.all([onLoadUsers()]);

  return {
    users: users,
  };
};

const UserAdministrationPageContainer: React.FC = () => {
  const { authenticationState } = useAuth();
  const { getResource } = useI18n();
  const [selectedUser, setSelectedUser] = React.useState<number>(1);

  const { initProps } = useComponentInitialization<UserAdministrationPageProps>(
    authenticationState.token,
    initializeAsync
  );

  const onSelectedUserChanged = React.useCallback((id: number) => {
    setSelectedUser(id);
  }, []);

  const userListItems = React.useMemo((): SettingsListItem[] => {
    return initProps?.users
      .sort((a, b) => (a.userId > b.userId ? 1 : -1))
      .map((user) => {
        return {
          id: user.userId,
          label: `${user.firstName} ${user.lastName}`,
          description: getResource("administration.labelDetails"),
          selected: selectedUser === user.userId,
          readonly: selectedUser === user.userId,
          onSectionChanged: onSelectedUserChanged,
        };
      });
  }, [selectedUser, initProps?.users, onSelectedUserChanged, getResource]);

  if (initProps?.users == null) {
    return null;
  }

  return (
    <Box width="100%" height="100%">
      <Box padding={4}>
        <SettingsLayout listItems={userListItems} selectedItem={selectedUser}>
          <Box height="100%">
            {initProps?.users.map((user, key) => (
              <UserDataDetails
                key={key}
                isOwnAccount={
                  authenticationState.jwtData.userId === user.userId
                }
                selectedUserId={selectedUser}
                user={user}
              />
            ))}
          </Box>
        </SettingsLayout>
      </Box>
    </Box>
  );
};

export default UserAdministrationPageContainer;
