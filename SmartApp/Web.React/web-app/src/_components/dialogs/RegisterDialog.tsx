import {
  Dialog,
  DialogActions,
  DialogContent,
  Grid2,
  Typography,
} from "@mui/material";
import React from "react";
import { useI18n } from "src/_hooks/useI18n";
import { AccountRequest } from "src/_lib/_types/auth";
import TextInput from "../_input/TextInput";
import FormButton from "../_buttons/FormButton";
import { useAuth } from "src/_hooks/useAuth";
import { emailValidation, passwordValidation } from "src/_lib/validation";
import { colors } from "src/_lib/colors";
import DialogLoadingIndicator from "./DialogLoadingIndicator";
import { PermIdentityRounded } from "@mui/icons-material";

interface IProps {
  open: boolean;
  onClose: () => void;
}

const RegisterDialog: React.FC<IProps> = (props) => {
  const { open, onClose } = props;
  const { getResource } = useI18n();
  const { isLoading, onRegister } = useAuth();

  const [registerModel, setRegisterModel] = React.useState<AccountRequest>({
    firstName: "",
    lastName: "",
    email: "",
    password: "",
    passwordValidation: "",
  });

  React.useEffect(() => {
    if (open)
      setRegisterModel({
        firstName: "",
        lastName: "",
        email: "",
        password: "",
        passwordValidation: "",
      });
  }, [open]);

  const handleFirstNameChanged = React.useCallback(
    (value: string) => {
      setRegisterModel({ ...registerModel, firstName: value });
    },
    [registerModel]
  );

  const handleLastNameChanged = React.useCallback(
    (value: string) => {
      setRegisterModel({ ...registerModel, lastName: value });
    },
    [registerModel]
  );

  const handleEmailChanged = React.useCallback(
    (value: string) => {
      setRegisterModel({ ...registerModel, email: value });
    },
    [registerModel]
  );

  const handlePasswordChanged = React.useCallback(
    (value: string) => {
      setRegisterModel({ ...registerModel, password: value });
    },
    [registerModel]
  );

  const handlePasswordValidationChanged = React.useCallback(
    (value: string) => {
      setRegisterModel({ ...registerModel, passwordValidation: value });
    },
    [registerModel]
  );

  const handleCancel = React.useCallback(() => {
    setRegisterModel({
      firstName: "",
      lastName: "",
      email: "",
      password: "",
      passwordValidation: "",
    });
    onClose();
  }, [onClose]);

  const handleRegistration = React.useCallback(async () => {
    await onRegister(registerModel).then((res) => {
      if (res === true) {
        handleCancel();
      }
    });
  }, [registerModel, onRegister, handleCancel]);

  const loginDisabled =
    !emailValidation(registerModel.email) ||
    (!passwordValidation(registerModel.password) &&
      registerModel.password !== registerModel.passwordValidation);

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
          <PermIdentityRounded
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
            {getResource("common.labelCreateAccount")}
          </Typography>
        </Grid2>
        <DialogLoadingIndicator isLoading={isLoading} />
        <Grid2 marginTop="10px">
          <TextInput
            label={getResource("common.labelFirstName")}
            value={registerModel.firstName}
            fullwidth={true}
            onChange={handleFirstNameChanged}
          />
        </Grid2>
        <Grid2 marginTop="10px">
          <TextInput
            label={getResource("common.labelLastName")}
            value={registerModel.lastName}
            fullwidth={true}
            onChange={handleLastNameChanged}
          />
        </Grid2>
        <Grid2 marginTop="10px">
          <TextInput
            label={getResource("common.labelEmail")}
            value={registerModel.email}
            fullwidth={true}
            onChange={handleEmailChanged}
          />
        </Grid2>
        <Grid2 marginTop="10px">
          <TextInput
            label={getResource("common.labelPassword")}
            value={registerModel.password}
            isPassword
            fullwidth={true}
            onChange={handlePasswordChanged}
          />
        </Grid2>
        <Grid2 marginTop="10px">
          <TextInput
            label={getResource("common.labelPasswordValidation")}
            value={registerModel.passwordValidation}
            isPassword
            fullwidth={true}
            onChange={handlePasswordValidationChanged}
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
          onAction={handleCancel}
        />
        <FormButton
          label={getResource("common.labelRegister")}
          disabled={loginDisabled}
          onAction={handleRegistration}
        />
      </DialogActions>
    </Dialog>
  );
};

export default RegisterDialog;
