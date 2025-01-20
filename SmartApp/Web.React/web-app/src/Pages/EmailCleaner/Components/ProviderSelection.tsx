import { Box, Grid2 } from "@mui/material";
import React from "react";
import Dropdown, { DropDownItem } from "src/_components/Input/Dropdown";
import { useI18n } from "src/_hooks/useI18n";
import { EmailProviderTypeEnum } from "src/_lib/_enums/EmailProviderTypeEnum";
import { emailProviderSettings } from "src/_lib/Settings/EmailProviderSettings";
import { EmailAccountModel } from "../EmailCleanerTypes";

interface IProps {
  state: EmailAccountModel;
  disabled: boolean;
  handleChange: (partialAccount: Partial<EmailAccountModel>) => void;
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

      handleChange({
        server: providerSettings.server,
        port: providerSettings.port,
        providerType: providerSettings.type,
        emailAddress:
          type === EmailProviderTypeEnum.None ? "" : state.emailAddress,
        password: type === EmailProviderTypeEnum.None ? "" : state.password,
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
            fullWidth
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

export default ProviderSelection;
