import { AccountCircleRounded } from "@mui/icons-material";
import { Box, Paper, Typography } from "@mui/material";
import React from "react";
import { useNavigate } from "react-router-dom";
import FormButton from "src/_components/Buttons/FormButton";
import TextInput from "src/_components/Input/TextInput";
import LoadingIndicator from "src/_components/Loading/LoadingIndicator";
import { useAuth } from "src/_hooks/useAuth";
import { useI18n } from "src/_hooks/useI18n";
import { LoginData } from "src/_lib/_types/auth";
import { colors } from "src/_lib/colors";
import { emailValidation, passwordValidation } from "src/_lib/validation";
import background from "../../_lib/_img/pier.jpg";

const LoginPage: React.FC = () => {
  const { getResource } = useI18n();
  const { isLoading, onLogin } = useAuth();
  const navigate = useNavigate();

  const [loginModel, setLoginModel] = React.useState<LoginData>({
    email: "",
    password: "",
  });

  React.useEffect(() => {
    setLoginModel({ email: "", password: "" });
  }, []);

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

  const handleCancel = React.useCallback(() => {
    setLoginModel({ email: "", password: "" });
    navigate("/");
  }, [navigate]);

  const handleLogin = React.useCallback(async () => {
    await onLogin(loginModel).then((res) => {
      if (res === true) {
        navigate("/");
      }
    });
  }, [loginModel, onLogin, navigate]);

  const loginDisabled =
    !emailValidation(loginModel.email) ||
    !passwordValidation(loginModel.password);

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
      <Box width="30%" height="30%">
        <Paper sx={{ padding: 4, opacity: 0.7 }}>
          <Box
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
          </Box>
          <Box
            width="100%"
            display="flex"
            justifyContent="center"
            alignItems="center"
            marginTop="5px"
          >
            <Typography color={colors.typography.blue} variant="h5">
              {getResource("common.labelLogin")}
            </Typography>
          </Box>
          <LoadingIndicator isLoading={isLoading} />
          <Box marginTop="10px">
            <TextInput
              label={getResource("common.labelEmail")}
              value={loginModel.email}
              fullwidth={true}
              onChange={handleEmailChanged}
            />
          </Box>
          <Box marginTop="10px">
            <TextInput
              label={getResource("common.labelPassword")}
              value={loginModel.password}
              isPassword
              fullwidth={true}
              onChange={handlePasswordChanged}
            />
          </Box>
          <Box display="flex" justifyContent="flex-end" gap={2} padding={2}>
            <FormButton
              label={getResource("common.labelCancel")}
              disabled={loginModel.email === "" && loginModel.password === ""}
              onAction={handleCancel}
            />
            <FormButton
              label={getResource("common.labelLogin")}
              disabled={loginDisabled}
              onAction={handleLogin}
            />
          </Box>
        </Paper>
      </Box>
    </Box>
  );
};

export default LoginPage;
