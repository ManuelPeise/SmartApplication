import { Grid2, Typography } from "@mui/material";
import React from "react";
import TextInput from "src/_components/_input/TextInput";
import { LoginData } from "src/_lib/_types/auth";
import { useI18n } from "src/_hooks/useI18n";
import { AccountCircleRounded } from "@material-ui/icons";
import {
  formHeaderIconBase,
  formRowBase,
  formTitleStyleBase,
  iconFormRowBase,
  loginForm,
} from "src/_lib/_styles/formStyles";
import FormButton from "src/_components/_buttons/FormButton";
interface IProps {
  tabIndex: number;
  selectedTabIndex: number;
  onLogin: (model: LoginData) => Promise<void>;
  onSelectedTabChanged: (tabindex: number) => void;
}

const LoginTab: React.FC<IProps> = (props) => {
  const { tabIndex, selectedTabIndex, onSelectedTabChanged, onLogin } = props;

  const { getResource } = useI18n();

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

  const handleLogin = React.useCallback(() => {
    onLogin(loginModel);
  }, [loginModel, onLogin]);

  if (selectedTabIndex !== tabIndex) {
    return null;
  }

  return (
    <Grid2 style={loginForm}>
      <Grid2 style={iconFormRowBase}>
        <AccountCircleRounded
          style={{ ...formHeaderIconBase, width: "5rem", height: "5rem" }}
        />
      </Grid2>
      <Grid2 style={formRowBase}>
        <Typography style={formTitleStyleBase}>
          {getResource("common.labelLogin")}
        </Typography>
      </Grid2>
      <Grid2 style={formRowBase}>
        <TextInput
          label={getResource("common.labelEmail")}
          value={loginModel.email}
          fullwidth={true}
          onChange={handleEmailChanged}
        />
      </Grid2>
      <Grid2 style={formRowBase}>
        <TextInput
          label={getResource("common.labelPassword")}
          value={loginModel.password}
          isPassword
          fullwidth={true}
          onChange={handlePasswordChanged}
        />
      </Grid2>
      <Grid2
        style={{
          ...formRowBase,
          justifyContent: "flex-end",
          gap: 8,
          marginTop: "1rem",
        }}
      >
        <FormButton
          label={getResource("common.labelCreateAccount")}
          onAction={onSelectedTabChanged.bind(null, 1)}
        />
        <FormButton
          label={getResource("common.labelLogin")}
          onAction={handleLogin}
        />
      </Grid2>
    </Grid2>
  );
};

export default LoginTab;
