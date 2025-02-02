import {
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
} from "@mui/material";
import React from "react";
import FormButton from "../Buttons/FormButton";
import { useI18n } from "src/_hooks/useI18n";

interface IProps {
  open: boolean;
  agreed: boolean;
  agreementText: string;
  onClose: () => void;
  onAgree: () => void;
}

const AgreementDialog: React.FC<IProps> = (props) => {
  const { open, agreed, agreementText, onClose, onAgree } = props;
  const { getResource } = useI18n();

  const handleAgree = React.useCallback(() => {
    onAgree();
    onClose();
  }, [onAgree, onClose]);

  return (
    <Dialog open={open}>
      <DialogContent>
        <DialogContentText sx={{ whiteSpace: "break-spaces" }}>
          {agreementText}
        </DialogContentText>
      </DialogContent>
      <DialogActions>
        <FormButton
          label={getResource("common.labelClose")}
          onAction={onClose}
        />
        <FormButton
          label={getResource("common.labelAgree")}
          disabled={agreed}
          onAction={handleAgree}
        />
      </DialogActions>
    </Dialog>
  );
};

export default AgreementDialog;
