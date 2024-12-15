import { Dialog, DialogActions, DialogContent, Grid2 } from "@mui/material";
import React from "react";
import FormButton from "src/_components/_buttons/FormButton";
import TextInput from "src/_components/_input/TextInput";
import { useI18n } from "src/_hooks/useI18n";
import { EmailAccountSettings } from "../_types/Settings";

interface IProps {
  open: boolean;
  data: EmailAccountSettings;
  onAction: () => void;
  onCancel: () => void;
}

const EmailAccountServerSettingsDialog: React.FC<IProps> = (props) => {
  const { open, data, onAction, onCancel } = props;
  const { getResource } = useI18n();
  return (
    <Dialog
      open={open}
      keepMounted
      fullWidth
      maxWidth={"sm"}
      style={{ padding: "20px" }}
    >
      <DialogContent
        style={{
          display: "flex",
          justifyContent: "center",
          flexDirection: "column",
          padding: "25px 30px",
        }}
      >
        <Grid2 display="flex" justifyContent="center" gap={6}>
          <TextInput
            label={getResource("settings.labelServerAddress")}
            value={data.emailServerAddress ?? ""}
            fullwidth={false}
            onChange={() => {}}
          />
          <TextInput
            label={getResource("settings.labelServerPort")}
            value={data?.port?.toString() ?? ""}
            fullwidth={false}
            onChange={() => {}}
          />
        </Grid2>
        <Grid2 display="flex" justifyContent="center" gap={6}>
          <TextInput
            label={getResource("settings.labelEmailAddress")}
            value={data.emailAddress ?? ""}
            fullwidth={false}
            onChange={() => {}}
          />
          <TextInput
            label={getResource("settings.labelPassword")}
            value={data.password ?? ""}
            isPassword
            fullwidth={false}
            onChange={() => {}}
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
          label={getResource("common.labelSave")}
          onAction={onAction}
        />
      </DialogActions>
    </Dialog>
  );
};

export default EmailAccountServerSettingsDialog;
