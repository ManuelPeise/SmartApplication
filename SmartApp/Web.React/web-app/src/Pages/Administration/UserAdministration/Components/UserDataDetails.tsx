import React from "react";
import { UserAdministrationUserModel } from "../Types/UserAdministrationUserModel";
import {
  Box,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  List,
  Switch,
} from "@mui/material";
import ListItemInput from "src/_components/Lists/ListItemInput";
import FormButton from "src/_components/Buttons/FormButton";
import { useI18n } from "src/_hooks/useI18n";
import { isEqual } from "lodash";
import { AccessRight } from "src/_lib/_types/auth";
import UserRightList from "./UserRightList";
import { groupByKey, sortBy } from "src/_lib/utils";
import { useAccessRights } from "src/_hooks/useAccessRights";
import { useAuth } from "src/_hooks/useAuth";
import { useEditFormState } from "src/_hooks/useEditFormState";

interface IProps {
  selectedUserId: number;
  tempUserId: number;
  user: UserAdministrationUserModel;
  onResetTempUserId: React.Dispatch<React.SetStateAction<number>>;
  onSelectedUserChanged: (id: number) => void;
  onUpdateUser: (model: UserAdministrationUserModel) => Promise<boolean>;
  onUpdateUsersState: (models: UserAdministrationUserModel) => void;
}

const UserDataDetails: React.FC<IProps> = (props) => {
  const {
    selectedUserId,
    tempUserId,
    user,
    onResetTempUserId,
    onUpdateUser,
    onUpdateUsersState,
    onSelectedUserChanged,
  } = props;

  const { authenticationState } = useAuth();
  const { getResource } = useI18n();
  const originalState = React.useRef(user);

  const {
    modelState,
    formState,
    handleRevertChangesAndCloseDialog,
    handleModelStateChanged,
    handleCloseDialog,
  } = useEditFormState<UserAdministrationUserModel>(user, tempUserId);

  const { getAccessRight } = useAccessRights();

  const readOnly =
    authenticationState.jwtData.userId.toString() === user.userId.toString() ||
    !getAccessRight("UserAdministration").canEdit;

  const handleAccessRightChanged = React.useCallback(
    (accessRightUpdate: AccessRight, id: number) => {
      const accessRights = modelState.accessRights.slice();

      const index = accessRights.findIndex((x) => x.id === id);

      if (index !== -1) {
        accessRights[index] = accessRightUpdate;

        handleModelStateChanged({
          ...modelState,
          accessRights: accessRights,
        });
      }
    },
    [modelState, handleModelStateChanged]
  );

  const handleReset = React.useCallback(() => {
    handleModelStateChanged(originalState.current);
  }, [originalState, handleModelStateChanged]);

  const handleSave = React.useCallback(async () => {
    await onUpdateUser(modelState).then((res) => {
      if (res === true) {
        originalState.current = modelState;
        onUpdateUsersState(modelState);
      }
    });
  }, [modelState, onUpdateUser, onUpdateUsersState]);

  const userRights = React.useMemo(() => {
    const sortedRights = sortBy(modelState.accessRights, "group", "dsc");

    return groupByKey(sortedRights, "group");
  }, [modelState?.accessRights]);

  if (selectedUserId !== user.userId) {
    return null;
  }

  return (
    <Box
      id={`user-details-${user.userId}`}
      sx={{
        display: "flex",
        flexDirection: "column",
        justifyContent: "space-between",
        height: "100%",
        minHeight: "750px",
        padding: 4,
      }}
    >
      <List disablePadding>
        <ListItemInput
          additionalFontSize="1.3rem"
          marginTop=".8rem"
          additionalFontStyle="italic"
          additionalFontWeight="600"
          label={getResource("administration.descriptionUserDetails").replace(
            "{User}",
            `${user.firstName} ${user.lastName}`
          )}
        ></ListItemInput>
        <ListItemInput
          label={getResource("administration.descriptionIsActive")}
        >
          <Switch
            disabled={readOnly}
            checked={modelState.isActive}
            color="success"
            onChange={(e) =>
              handleModelStateChanged({ isActive: e.currentTarget.checked })
            }
          />
        </ListItemInput>
        <ListItemInput label={getResource("administration.descriptionIsAdmin")}>
          <Switch
            disabled={readOnly}
            checked={modelState.isAdmin}
            color="success"
            onChange={(e) =>
              handleModelStateChanged({ isAdmin: e.currentTarget.checked })
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
            disabled={isEqual(originalState.current, modelState)}
            onAction={handleReset}
          />
          <FormButton
            label={getResource("common.labelSave")}
            disabled={isEqual(originalState.current, modelState)}
            onAction={handleSave}
          />
        </Box>
      </Box>
      <Dialog open={formState.isModified && user.userId !== tempUserId}>
        <DialogContent>
          <DialogContentText>
            {getResource("administration.labelUnsavedChanges")}
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button
            onClick={() =>
              handleCloseDialog(onResetTempUserId.bind(null, user.userId))
            }
          >
            {getResource("administration.labelClose")}
          </Button>
          <Button
            onClick={() =>
              handleRevertChangesAndCloseDialog(onSelectedUserChanged)
            }
          >
            {getResource("administration.labelRevertChangesAndClose")}
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
};

export default UserDataDetails;
