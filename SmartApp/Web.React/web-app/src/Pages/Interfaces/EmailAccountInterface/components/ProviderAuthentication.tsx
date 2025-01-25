import React from "react";
import { EmailAccountSettings } from "../types";
import { Grid2 } from "@mui/material";
import TextInput from "src/_components/Input/TextInput";
import { useI18n } from "src/_hooks/useI18n";
import { EmailProviderTypeEnum } from "src/_lib/_enums/EmailProviderTypeEnum";
import { emailValidation } from "src/_lib/validation";

interface IProps {
  state: EmailAccountSettings;
  handleChange: (partialSettings: Partial<EmailAccountSettings>) => void;
}

const ProviderAuthentication: React.FC<IProps> = (props) => {
  const { state, handleChange } = props;
  const { getResource } = useI18n();

  return (
    <Grid2
      display="flex"
      width="inherit"
      justifyContent="flex-end"
      alignItems="flex-end"
      paddingTop={4}
      gap={4}
    >
      <Grid2 minWidth={300}>
        <TextInput
          fullwidth
          disabled={
            state.providerType === EmailProviderTypeEnum.None ||
            state.accountName === ""
          }
          label={getResource("interface.labelEmailAddress")}
          value={state.emailAddress}
          onChange={(value) =>
            handleChange({ emailAddress: value, connectionTestPassed: false })
          }
        />
      </Grid2>
      <Grid2 minWidth={300}>
        <TextInput
          fullwidth
          isPassword
          disabled={
            state.providerType === EmailProviderTypeEnum.None ||
            state.accountName === "" ||
            !emailValidation(state.emailAddress)
          }
          label={getResource("interface.labelPassword")}
          value={state?.password ?? ""}
          onChange={(value) =>
            handleChange({ password: value, connectionTestPassed: false })
          }
        />
      </Grid2>
    </Grid2>
  );
};

export default ProviderAuthentication;
