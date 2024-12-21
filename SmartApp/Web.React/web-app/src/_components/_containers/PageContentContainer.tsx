import {
  Box,
  Drawer,
  IconButton,
  List,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Typography,
} from "@mui/material";
import {
  ArrowBackRounded,
  ReportProblemRounded,
  EmailRounded,
} from "@material-ui/icons";
import React, { PropsWithChildren } from "react";
import { SideMenu } from "src/_lib/_types/menu";
import { Link } from "react-router-dom";
import { useAuth } from "src/_hooks/useAuth";
import { UserRoleEnum } from "src/_lib/_enums/UserRoleEnum";
import { routes } from "src/_lib/AppRouter";
import { colors } from "src/_lib/colors";
import { useI18n } from "src/_hooks/useI18n";

interface IProps extends PropsWithChildren {
  sideMenuOpen: boolean;
  onClose: React.Dispatch<React.SetStateAction<boolean>>;
}

const PageContentContainer: React.FC<IProps> = (props) => {
  const { children, sideMenuOpen, onClose } = props;
  const { authenticationState } = useAuth();
  const { getResource } = useI18n();

  const appSideMenu = React.useMemo((): SideMenu => {
    return {
      items: [
        {
          displayName: getResource("common.labelLogging"),
          route: routes.log,
          disabled:
            authenticationState == null ||
            authenticationState.jwtData.userRole !== UserRoleEnum.Admin,
          icon: (
            <ReportProblemRounded fontSize="small" style={{ color: "#fff" }} />
          ),
        },
        {
          displayName: getResource("common.labelEmailProviderConfituration"),
          route: routes.emailProviderConfiguration,
          disabled: authenticationState == null,
          icon: <EmailRounded fontSize="small" style={{ color: "#fff" }} />,
        },
      ],
    };
  }, [authenticationState, getResource]);

  return (
    <Box
      display="flex"
      flexDirection="row"
      height="100%"
      bgcolor={colors.background}
    >
      <Drawer
        anchor="left"
        open={sideMenuOpen}
        slotProps={{
          backdrop: {
            sx: {
              backgroundColor: "transparent",
              opacity: 0.5,
            },
          },
        }}
        sx={{ zIndex: 1000 }}
      >
        <Box
          display="flex"
          justifyContent="flex-end"
          padding={2}
          bgcolor="primary.dark"
        >
          <IconButton
            size="small"
            color="secondary"
            onClick={onClose.bind(null, false)}
          >
            <ArrowBackRounded />
          </IconButton>
        </Box>
        <Box height="100%" width="300px" bgcolor="primary.dark">
          <List
            disablePadding
            style={{
              width: "100%",
            }}
          >
            {appSideMenu.items.map((item, key) => {
              return (
                <ListItemButton
                  key={key}
                  sx={{
                    borderBottom: "1px solid lightgray",
                  }}
                >
                  <ListItemIcon>{item.icon}</ListItemIcon>
                  <ListItemText>
                    <Link
                      to={item.route}
                      style={{ textDecoration: "none" }}
                      onClick={onClose.bind(null, false)}
                    >
                      <Typography variant="inherit" color="secondary">
                        {item.displayName}
                      </Typography>
                    </Link>
                  </ListItemText>
                </ListItemButton>
              );
            })}
          </List>
        </Box>
      </Drawer>
      {/* content */}
      <Box display="flex" width="100%">
        {children}
      </Box>
    </Box>
  );
};

export default PageContentContainer;
