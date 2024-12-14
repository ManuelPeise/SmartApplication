import { AccountCircleRounded } from "@material-ui/icons";
import {
  Dialog,
  DialogActions,
  DialogContent,
  Grid2,
  Typography,
} from "@mui/material";
import React from "react";
import { useI18n } from "src/_hooks/useI18n";
import { LoginData } from "src/_lib/_types/auth";
import TextInput from "../_input/TextInput";
import FormButton from "../_buttons/FormButton";
import { useAuth } from "src/_hooks/useAuth";
import { emailValidation, passwordValidation } from "src/_lib/validation";
import { colors } from "src/_lib/colors";
import DialogLoadingIndicator from "./DialogLoadingIndicator";

interface IProps {
  open: boolean;
  onCancel: () => void;
  onClose: () => void;
}

const AuthenticationDialog: React.FC<IProps> = (props) => {
  const { open, onCancel, onClose } = props;
  const { getResource } = useI18n();
  const { isLoading, onLogin } = useAuth();
  const [loginModel, setLoginModel] = React.useState<LoginData>({
    email: "",
    password: "",
  });

  React.useEffect(() => {
    if (open) setLoginModel({ email: "", password: "" });
  }, [open]);

  const handleEmailChanged = React.useCallback(
    (value: string) => {
      setLoginModel({ ...loginModel, email: value });
    },
    [loginModel]
  );

  const handlePasswordChanged = React.useCallback(
    (value: string) => {
      setLoginModel({ ...loginModel, password: value });
    },
    [loginModel]
  );

  const handleLogin = React.useCallback(async () => {
    await onLogin(loginModel).then((res) => {
      if (res === true) {
        onClose();
      }
    });
  }, [loginModel, onLogin, onClose]);

  const loginDisabled =
    !emailValidation(loginModel.email) ||
    !passwordValidation(loginModel.password);

  return (
    <Dialog open={open} fullWidth maxWidth={"sm"} style={{ padding: "20px" }}>
      <DialogContent
        style={{
          display: "flex",
          justifyContent: "center",
          flexDirection: "column",
          padding: "25px 30px",
        }}
      >
        <Grid2
          width="100%"
          display="flex"
          justifyContent="center"
          alignItems="center"
        >
          <AccountCircleRounded
            style={{
              width: "3.5rem",
              height: "3.5rem",
              color: colors.icons.blue,
            }}
          />
        </Grid2>
        <Grid2
          width="100%"
          display="flex"
          justifyContent="center"
          alignItems="center"
          marginTop="5px"
        >
          <Typography color={colors.typography.blue} variant="h5">
            {getResource("common.labelLogin")}
          </Typography>
        </Grid2>
        <DialogLoadingIndicator isLoading={isLoading} />
        <Grid2 marginTop="10px">
          <TextInput
            label={getResource("common.labelEmail")}
            value={loginModel.email}
            fullwidth={true}
            onChange={handleEmailChanged}
          />
        </Grid2>
        <Grid2 marginTop="10px">
          <TextInput
            label={getResource("common.labelPassword")}
            value={loginModel.password}
            isPassword
            fullwidth={true}
            onChange={handlePasswordChanged}
          />
        </Grid2>
      </DialogContent>
      <DialogActions
        style={{
          display: "flex",
          justifyContent: "flex-end",
          gap: 10,
          padding: "0px 40px 25px 20px",
        }}
      >
        <FormButton
          label={getResource("common.labelCancel")}
          onAction={onCancel}
        />
        <FormButton
          label={getResource("common.labelLogin")}
          disabled={loginDisabled}
          onAction={handleLogin}
        />
      </DialogActions>
    </Dialog>
  );
};

export default AuthenticationDialog;
