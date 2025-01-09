import {
  Box,
  Divider,
  List,
  ListItemButton,
  ListItemText,
  Paper,
  Typography,
} from "@mui/material";
import React from "react";
import { UserAdministrationUserModel } from "./Types/UserAdministrationUserModel";
import { AccessRight } from "src/_lib/_types/auth";
import { UserListItemProps } from "./Types/UserListItemProps";
import { useI18n } from "src/_hooks/useI18n";
import UserDataDetails from "./Components/UserDataDetails";
import LoadingIndicator from "src/_components/Loading/LoadingIndicator";
import { StatelessApiOptions } from "src/_lib/_api/StatelessApi";

interface IProps {
  accessRight: AccessRight;
  users: UserAdministrationUserModel[];
  isLoading: boolean;
  sendPost: (apiOptions: StatelessApiOptions) => Promise<boolean>;
  onRebind: () => Promise<void>;
}

const UserAdministrationPage: React.FC<IProps> = (props) => {
  const { accessRight, users, isLoading, sendPost, onRebind } = props;
  const { getResource } = useI18n();
  const [selectedIndex, setSelectedIndex] = React.useState<number>(0);

  const handleSelectedIndex = React.useCallback((index: number) => {
    setSelectedIndex(index);
  }, []);

  const onUpdateUser = React.useCallback(
    async (user: UserAdministrationUserModel) => {
      await sendPost({
        serviceUrl: "UserAdministration/UpdateUser",
        body: user,
      }).then(async () => {
        onRebind();
      });
    },
    [sendPost, onRebind]
  );

  const userListItems = React.useMemo((): UserListItemProps[] => {
    return users.map((usr, index) => {
      return {
        index: index,
        label: `${usr.firstName} ${usr.lastName}`,
        description: getResource("administration.labelDetails"),
        disabled: index === selectedIndex,
        onClick: handleSelectedIndex.bind(null, index),
      };
    });
  }, [users, selectedIndex, getResource, handleSelectedIndex]);

  return (
    <Box
      width="100%"
      height="100%"
      display="flex"
      justifyContent="center"
      alignItems="center"
    >
      <Box width="98%" height="98%">
        <LoadingIndicator isLoading={isLoading} />
        <Paper sx={{ width: "100%", height: "100%" }} elevation={3}>
          <Box width="100%" height="100%" display="flex" flexDirection="row">
            <Box width="100%" maxWidth="300px" height="100%">
              <List disablePadding>
                {userListItems.map((item) => (
                  <ListItemButton
                    key={item.index}
                    disabled={item.disabled}
                    selected={selectedIndex === item.index}
                    onClick={item.onClick}
                  >
                    <ListItemText
                      secondary={<Typography>{item.description}</Typography>}
                    >
                      {item.label}
                    </ListItemText>
                  </ListItemButton>
                ))}
              </List>
            </Box>
            <Divider orientation="vertical" />
            <Box width="100%" height="100%">
              <UserDataDetails
                user={users[selectedIndex]}
                requiredAccessRight={accessRight}
                onUpdateUser={onUpdateUser}
              />
            </Box>
          </Box>
        </Paper>
      </Box>
    </Box>
  );
};

export default UserAdministrationPage;
