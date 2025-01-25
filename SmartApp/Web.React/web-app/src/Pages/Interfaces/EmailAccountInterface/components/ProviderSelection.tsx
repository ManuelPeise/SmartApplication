import { Box, Grid2 } from "@mui/material";
import React from "react";
import Dropdown, { DropDownItem } from "src/_components/Input/Dropdown";
import { useI18n } from "src/_hooks/useI18n";
import { EmailProviderTypeEnum } from "src/_lib/_enums/EmailProviderTypeEnum";
import { emailProviderSettings } from "src/_lib/Settings/EmailProviderSettings";
import { EmailAccountSettings } from "../types";

interface IProps {
  state: EmailAccountSettings;
  disabled: boolean;
  handleChange: (partialAccount: Partial<EmailAccountSettings>) => void;
}

const ProviderSelection: React.FC<IProps> = (props) => {
  const { state, disabled, handleChange } = props;
  const { getResource } = useI18n();

  const providerSettings = React.useMemo(() => {
    return emailProviderSettings.find((x) => x.type === state.providerType);
  }, [state]);

  const providerDropdownItems = React.useMemo((): DropDownItem[] => {
    const items: DropDownItem[] = emailProviderSettings.map((s, index) => {
      return {
        key: index,
        label: getResource(s.resourceKey),
      };
    });

    return items;
  }, [getResource]);

  const handleProviderChanged = React.useCallback(
    (type: EmailProviderTypeEnum) => {
      const providerSettings = emailProviderSettings.find(
        (x) => x.type === type
      );

      if (type === EmailProviderTypeEnum.None) {
        handleChange({
          accountName: "",
          server: "",
          port: -1,
          providerType: EmailProviderTypeEnum.None,
          emailAddress: "",
          password: "",
          connectionTestPassed: false,
        });
        return;
      }

      handleChange({
        server: providerSettings.server,
        port: providerSettings.port,
        providerType: providerSettings.type,
        emailAddress: state.emailAddress,
        password: state.password,
      });
    },
    [state, handleChange]
  );

  return (
    <Grid2 size={12}>
      <Grid2
        display="flex"
        flexDirection="row"
        alignItems="flex-end"
        gap={2}
        p={1}
        justifyContent="space-between"
        width="100%"
      >
        <Box display="flex" flexDirection="row" alignItems="flex-end" gap={4}>
          <Box
            component="img"
            src={providerSettings.imageSrc}
            alt="provider logo"
            sx={{ width: "50px", height: "50px" }}
          />
        </Box>
        <Box
          width="50%"
          minWidth="10rem"
          display="flex"
          flexDirection="row"
          alignItems="flex-end"
          justifyContent="flex-end"
          gap={2}
        >
          <Dropdown
            minWidth={200}
            disabled={disabled}
            items={providerDropdownItems}
            value={state.providerType}
            onChange={(type) => handleProviderChanged(type)}
          />
        </Box>
      </Grid2>
    </Grid2>
  );
};

export default React.memo(ProviderSelection);
