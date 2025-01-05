import { Box, Paper, Typography } from "@mui/material";
import React from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "src/_hooks/useAuth";
import { useI18n } from "src/_hooks/useI18n";
import background from "../../_lib/_img/pier.jpg";
import { AccountRequest } from "src/_lib/_types/auth";
import { emailValidation, passwordValidation } from "src/_lib/validation";
import { PermIdentityRounded } from "@mui/icons-material";
import { colors } from "src/_lib/colors";
import LoadingIndicator from "src/_components/Loading/LoadingIndicator";
import TextInput from "src/_components/Input/TextInput";
import FormButton from "src/_components/Buttons/FormButton";

const RegisterPage: React.FC = () => {
  const { getResource } = useI18n();
  const { isLoading, onRegister } = useAuth();
  const navigate = useNavigate();

  const [registerModel, setRegisterModel] = React.useState<AccountRequest>({
    firstName: "",
    lastName: "",
    email: "",
    password: "",
    passwordValidation: "",
  });

  const onModelChanged = React.useCallback(
    (value: Partial<AccountRequest>) => {
      setRegisterModel({ ...registerModel, ...value });
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
    navigate("/");
  }, [navigate]);

  const handleRegistration = React.useCallback(async () => {
    await onRegister(registerModel).then((res) => {
      if (res === true) {
        handleCancel();
        navigate("/");
      }
    });
  }, [registerModel, onRegister, handleCancel, navigate]);

  React.useEffect(() => {
    setRegisterModel({
      firstName: "",
      lastName: "",
      email: "",
      password: "",
      passwordValidation: "",
    });
  }, []);

  const registerDisabled =
    !emailValidation(registerModel.email) ||
    !passwordValidation(registerModel.password) ||
    !registerModel.password.length ||
    registerModel.password !== registerModel.passwordValidation;

  return (
    <Box
      width="100%"
      height="100%"
      display="flex"
      justifyContent="center"
      alignItems="center"
      sx={{
        backgroundImage: `url(${background})`,
        backgroundPosition: "center",
        backgroundRepeat: "no-repeat",
      }}
    >
      <Box width="30%" height="auto">
        <Paper sx={{ padding: 4, opacity: 0.7 }}>
          <Box width="100%" display="flex" justifyContent="center">
            <PermIdentityRounded
              style={{
                width: "3.5rem",
                height: "3.5rem",
                color: colors.icons.blue,
              }}
            />
          </Box>
          <Box width="100%" display="flex" justifyContent="center">
            <Typography color={colors.typography.blue} variant="h5">
              {getResource("common.labelCreateAccount")}
            </Typography>
          </Box>
          <Box marginTop="2rem">
            <LoadingIndicator isLoading={isLoading} />
          </Box>
          <Box marginTop="10px">
            <TextInput
              label={getResource("common.labelFirstName")}
              value={registerModel.firstName}
              fullwidth={true}
              onChange={(value) => onModelChanged({ firstName: value })}
            />
          </Box>
          <Box marginTop="10px">
            <TextInput
              label={getResource("common.labelLastName")}
              value={registerModel.lastName}
              fullwidth={true}
              onChange={(value) => onModelChanged({ lastName: value })}
            />
          </Box>
          <Box marginTop="10px">
            <TextInput
              label={getResource("common.labelEmail")}
              value={registerModel.email}
              fullwidth={true}
              onChange={(value) => onModelChanged({ email: value })}
            />
          </Box>
          <Box marginTop="10px">
            <TextInput
              label={getResource("common.labelPassword")}
              value={registerModel.password}
              isPassword
              fullwidth={true}
              onChange={(value) => onModelChanged({ password: value })}
            />
          </Box>
          <Box marginTop="10px">
            <TextInput
              label={getResource("common.labelPasswordValidation")}
              value={registerModel.passwordValidation}
              isPassword
              fullwidth={true}
              onChange={(value) =>
                onModelChanged({ passwordValidation: value })
              }
            />
          </Box>
          <Box
            marginTop="1.5rem"
            display="flex"
            justifyContent="flex-end"
            gap={2}
          >
            <FormButton
              label={getResource("common.labelCancel")}
              onAction={handleCancel}
            />
            <FormButton
              label={getResource("common.labelRegister")}
              disabled={registerDisabled}
              onAction={handleRegistration}
            />
          </Box>
        </Paper>
      </Box>
    </Box>
  );
};

export default RegisterPage;
