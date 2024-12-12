import { Grid2, Typography } from "@mui/material";
import React from "react";
import TextInput from "src/_components/_input/TextInput";
import { AccountRequest } from "src/_lib/_types/auth";
import { useI18n } from "src/_hooks/useI18n";
import { PermIdentityRounded } from "@material-ui/icons";
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
  onSelectedTabChanged: (tabindex: number) => void;
}

const RegisterTab: React.FC<IProps> = (props) => {
  const { tabIndex, selectedTabIndex, onSelectedTabChanged } = props;

  const { getResource } = useI18n();
  const [registerModel, setRegisterModel] = React.useState<AccountRequest>({
    firstName: "",
    lastName: "",
    email: "",
  });

  React.useEffect(() => {
    setRegisterModel({ email: "", firstName: "", lastName: "" });
  }, []);

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

  if (selectedTabIndex !== tabIndex) {
    return null;
  }

  return (
    <Grid2 style={loginForm}>
      <Grid2 style={iconFormRowBase}>
        <PermIdentityRounded
          style={{ ...formHeaderIconBase, width: "5rem", height: "5rem" }}
        />
      </Grid2>
      <Grid2 style={formRowBase}>
        <Typography style={formTitleStyleBase}>
          {getResource("common.labelCreateAccount")}
        </Typography>
      </Grid2>
      <Grid2 style={formRowBase}>
        <TextInput
          label={getResource("common.labelFirstName")}
          value={registerModel.firstName}
          fullwidth={true}
          onChange={handleFirstNameChanged}
        />
      </Grid2>
      <Grid2 style={formRowBase}>
        <TextInput
          label={getResource("common.labelLastName")}
          value={registerModel.lastName}
          fullwidth={true}
          onChange={handleLastNameChanged}
        />
      </Grid2>
      <Grid2 style={formRowBase}>
        <TextInput
          label={getResource("common.labelEmail")}
          value={registerModel.email}
          fullwidth={true}
          onChange={handleEmailChanged}
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
          onAction={onSelectedTabChanged.bind(null, 0)}
        />
      </Grid2>
    </Grid2>
  );
};

export default RegisterTab;
