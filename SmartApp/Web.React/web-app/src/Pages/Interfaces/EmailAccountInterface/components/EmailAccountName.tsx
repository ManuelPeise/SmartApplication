import React from "react";
import { EmailAccountSettings } from "../types";
import { useI18n } from "src/_hooks/useI18n";
import { Grid2 } from "@mui/material";
import TextInput from "src/_components/Input/TextInput";
import { EmailProviderTypeEnum } from "src/_lib/_enums/EmailProviderTypeEnum";

interface IProps {
  state: EmailAccountSettings;
  handleChange: (partialState: Partial<EmailAccountSettings>) => void;
}

const EmailAccountName: React.FC<IProps> = (props) => {
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
          disabled={state.providerType === EmailProviderTypeEnum.None}
          label={getResource("interface.labelAccountName")}
          value={state.accountName}
          onChange={(value) => handleChange({ accountName: value })}
        />
      </Grid2>
    </Grid2>
  );
};

export default React.memo(EmailAccountName);
