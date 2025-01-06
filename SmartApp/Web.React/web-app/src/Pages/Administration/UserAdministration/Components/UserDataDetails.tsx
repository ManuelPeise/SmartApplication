import React from "react";
import { UserAdministrationUserModel } from "../Types/UserAdministrationUserModel";
import { Box, List, Switch } from "@mui/material";
import ListItemInput from "src/_components/Lists/ListItemInput";
import FormButton from "src/_components/Buttons/FormButton";
import { useI18n } from "src/_hooks/useI18n";
import { isEqual } from "lodash";
import { AccessRight } from "src/_lib/_types/auth";
import UserRightList from "./UserRightList";
import { groupByKey, sortBy } from "src/_lib/utils";
import { useAccessRights } from "src/_hooks/useAccessRights";

interface IProps {
  selectedUserId: number;
  user: UserAdministrationUserModel;
  isOwnAccount: boolean;
}

const UserDataDetails: React.FC<IProps> = (props) => {
  const { selectedUserId, user, isOwnAccount } = props;
  const { getResource } = useI18n();
  const originalState = React.useRef(user);
  const { getAccessRight } = useAccessRights();

  const readOnly = getAccessRight("UserAdministration").canEdit || isOwnAccount;
  const [intermediateState, setInterMediateState] =
    React.useState<UserAdministrationUserModel>(user);

  const handleChanged = React.useCallback(
    (update: Partial<UserAdministrationUserModel>) => {
      setInterMediateState({ ...intermediateState, ...update });
    },
    [intermediateState]
  );

  const handleAccessRightChanged = React.useCallback(
    (accessRightUpdate: AccessRight, id: number) => {
      const accessRights = intermediateState.accessRights.slice();

      const index = accessRights.findIndex((x) => x.id === id);

      if (index !== -1) {
        accessRights[index] = accessRightUpdate;

        setInterMediateState({
          ...intermediateState,
          accessRights: accessRights,
        });
      }
    },
    [intermediateState]
  );

  const handleReset = React.useCallback(() => {
    setInterMediateState(originalState.current);
  }, [originalState]);

  const userRights = React.useMemo(() => {
    const sortedRights = sortBy(intermediateState.accessRights, "group", "dsc");

    return groupByKey(sortedRights, "group");
  }, [intermediateState?.accessRights]);

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
            checked={intermediateState.isActive}
            color="success"
            onChange={(e) =>
              handleChanged({ isActive: e.currentTarget.checked })
            }
          />
        </ListItemInput>
        <ListItemInput label={getResource("administration.descriptionIsAdmin")}>
          <Switch
            disabled={readOnly}
            checked={intermediateState.isAdmin}
            color="success"
            onChange={(e) =>
              handleChanged({ isAdmin: e.currentTarget.checked })
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
            disabled={isEqual(originalState.current, intermediateState)}
            onAction={handleReset}
          />
          <FormButton
            label={getResource("common.labelSave")}
            disabled={isEqual(originalState.current, intermediateState)}
            onAction={() => {}}
          />
        </Box>
      </Box>
    </Box>
  );
};

export default UserDataDetails;
