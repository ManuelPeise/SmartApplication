import {
  Box,
  Checkbox,
  FormControlLabel,
  List,
  Typography,
} from "@mui/material";
import React from "react";
import ListItemInput from "src/_components/Lists/ListItemInput";
import { useI18n } from "src/_hooks/useI18n";
import { AccessRight } from "src/_lib/_types/auth";

interface IProps {
  readOnly: boolean;
  accessRights: Record<string, AccessRight[]>;
  handleAccessRightChanged: (
    accessRightUpdate: AccessRight,
    id: number
  ) => void;
}

const UserRightList: React.FC<IProps> = (props) => {
  const { readOnly, accessRights, handleAccessRightChanged } = props;
  const { getResource } = useI18n();

  return (
    <List
      dense
      sx={{
        paddingLeft: 2,
        paddingRight: 2,
        height: 400,
        overflowX: "scroll",
      }}
    >
      {Object.keys(accessRights).map((right, index) => (
        <Box key={right} marginTop="1rem">
          <Box>
            <Typography variant="h6">
              {getResource(`administration.label${right}`)}
            </Typography>
          </Box>
          <Box display="flex" flexDirection="column">
            {accessRights[right]
              .sort((a, b) => a.name.localeCompare(b.name))
              .map((r) => (
                <ListItemInput key={r.id} label={r.name}>
                  <Box
                    display="flex"
                    flexDirection="row"
                    justifyContent="flex-end"
                  >
                    <FormControlLabel
                      label={getResource("administration.labelView")}
                      control={
                        <Checkbox
                          disabled={readOnly}
                          checked={r.canView}
                          onChange={(e) =>
                            handleAccessRightChanged(
                              {
                                ...r,
                                canView: e.currentTarget.checked,
                                canEdit:
                                  e.currentTarget.checked === false
                                    ? false
                                    : r.canEdit,
                                deny: false,
                              },
                              r.id
                            )
                          }
                        />
                      }
                    />
                    <FormControlLabel
                      label={getResource("administration.labelEdit")}
                      control={
                        <Checkbox
                          disabled={readOnly}
                          checked={r.canEdit}
                          onChange={(e) =>
                            handleAccessRightChanged(
                              {
                                ...r,
                                canView:
                                  e.currentTarget.checked === true
                                    ? true
                                    : r.canView,
                                canEdit: e.currentTarget.checked,
                                deny: false,
                              },
                              r.id
                            )
                          }
                        />
                      }
                    />
                    <FormControlLabel
                      label={getResource("administration.labelDeny")}
                      control={
                        <Checkbox
                          disabled={readOnly}
                          checked={r.deny}
                          onChange={(e) =>
                            handleAccessRightChanged(
                              {
                                ...r,
                                deny: e.currentTarget.checked,
                                canView: !e.currentTarget.checked,
                                canEdit: !e.currentTarget.checked,
                              },
                              r.id
                            )
                          }
                        />
                      }
                    />
                  </Box>
                </ListItemInput>
              ))}
          </Box>
        </Box>
      ))}
    </List>
  );
};

export default UserRightList;
