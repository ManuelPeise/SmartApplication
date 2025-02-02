import React from "react";
import { UserAdministrationUserModel } from "../Types/UserAdministrationUserModel";
import { Box, List, Switch } from "@mui/material";
import ListItemInput from "src/_components/Lists/ListItemInput";
import FormButton from "src/_components/Buttons/FormButton";
import { useI18n } from "src/_hooks/useI18n";
import { AccessRight } from "src/_lib/_types/auth";
import UserRightList from "./UserRightList";
import { groupByKey, sortBy } from "src/_lib/utils";
import { useAuth } from "src/_hooks/useAuth";
import { isEqual } from "lodash";

interface IProps {
  user: UserAdministrationUserModel;
  requiredAccessRight: AccessRight;
  onUpdateUser: (model: UserAdministrationUserModel) => Promise<void>;
}

const UserDataDetails: React.FC<IProps> = (props) => {
  const { user, requiredAccessRight, onUpdateUser } = props;
  const { authenticationState } = useAuth();
  const { getResource } = useI18n();

  React.useEffect(() => {
    setSelectedUser(user);
  }, [user]);

  const [selectedUser, setSelectedUser] =
    React.useState<UserAdministrationUserModel>(user);

  const readOnly =
    authenticationState.jwtData.userId.toString() === user.userId.toString() ||
    !requiredAccessRight.canEdit;

  const userDataModified = React.useMemo(() => {
    return isEqual(user, selectedUser);
  }, [user, selectedUser]);

  const handleUserDataChanged = React.useCallback(
    (partialUser: Partial<UserAdministrationUserModel>) => {
      setSelectedUser({ ...selectedUser, ...partialUser });
    },
    [selectedUser]
  );

  const handleResetUser = React.useCallback(() => {
    setSelectedUser(user);
  }, [user]);

  const handleAccessRightChanged = React.useCallback(
    (accessRightUpdate: AccessRight, name: string) => {
      const accessRights = [...selectedUser.accessRights];

      const index = accessRights.findIndex((x) => x.name === name);

      if (index !== -1) {
        accessRights[index] = accessRightUpdate;

        handleUserDataChanged({
          accessRights: accessRights,
        });
      }
    },
    [handleUserDataChanged, selectedUser.accessRights]
  );

  const handleSave = React.useCallback(async () => {
    await onUpdateUser(selectedUser);
  }, [selectedUser, onUpdateUser]);

  const userRights = React.useMemo(() => {
    const sortedRights = sortBy(selectedUser.accessRights, "group", "dsc");

    return groupByKey(sortedRights, "group");
  }, [selectedUser?.accessRights]);

  return (
    <Box
      id={`user-details-${user.userId}`}
      sx={{
        display: "flex",
        flexDirection: "column",
        justifyContent: "space-between",
        height: 550,
        minHeight: 550,
      }}
    >
      <Box padding={3}>
        <List disablePadding>
          <ListItemInput
            additionalFontSize="1.3rem"
            marginTop=".8rem"
            additionalFontStyle="italic"
            additionalFontWeight="600"
            label={getResource("administration.descriptionUserDetails").replace(
              "{User}",
              `${selectedUser.firstName} ${selectedUser.lastName}`
            )}
          ></ListItemInput>
          <ListItemInput
            label={getResource("administration.descriptionIsActive")}
          >
            <Switch
              disabled={readOnly}
              checked={selectedUser.isActive}
              color="success"
              onChange={(e) =>
                handleUserDataChanged({ isActive: e.currentTarget.checked })
              }
            />
          </ListItemInput>
          <ListItemInput
            label={getResource("administration.descriptionIsAdmin")}
          >
            <Switch
              disabled={readOnly}
              checked={selectedUser.isAdmin}
              color="success"
              onChange={(e) =>
                handleUserDataChanged({ isAdmin: e.currentTarget.checked })
              }
            />
          </ListItemInput>
          <ListItemInput
            additionalFontSize="1.3rem"
            marginTop="2rem"
            additionalFontStyle="italic"
            additionalFontWeight="600"
            label={getResource("administration.descriptionAccessRights")}
          />
          <UserRightList
            accessRights={userRights}
            readOnly={readOnly}
            handleAccessRightChanged={handleAccessRightChanged}
          />
        </List>
        <Box height="25%">
          <Box
            width="100%"
            display="flex"
            flexDirection="row"
            justifyContent="flex-end"
            paddingTop={4}
            gap={2}
          >
            <FormButton
              label={getResource("common.labelCancel")}
              disabled={userDataModified}
              onAction={handleResetUser}
            />
            <FormButton
              label={getResource("common.labelSave")}
              disabled={userDataModified}
              onAction={handleSave}
            />
          </Box>
        </Box>
      </Box>
    </Box>
  );
};

export default UserDataDetails;
