import { Grid2 } from "@mui/material";
import React, { PropsWithChildren } from "react";
import FormButton from "../Buttons/FormButton";

export type ButtonProps = {
  label: string;
  onAction: () => Promise<void> | void;
  disabled?: boolean;
};

export type IDetailsViewProps = PropsWithChildren & {
  justifyContent?: "center" | "space-between";
  saveCancelButtonProps: ButtonProps[];
  additionalButtonProps?: ButtonProps[];
};

const DetailsView: React.FC<IDetailsViewProps> = (props) => {
  const {
    children,
    justifyContent,
    saveCancelButtonProps,
    additionalButtonProps,
  } = props;

  return (
    <Grid2
      container
      height="100%"
      size={12}
      display="flex"
      alignItems="space-between"
      justifyContent="center"
    >
      <Grid2
        size={12}
        height="90%"
        display="flex"
        overflow="scroll"
        justifyContent={justifyContent}
      >
        {children}
      </Grid2>
      <Grid2 size={12} height={30} display="flex" flexDirection="row">
        <Grid2 size={6} display="flex">
          {additionalButtonProps?.map((item, index) => (
            <FormButton
              key={`additional-button-${index}`}
              disabled={item.disabled}
              label={item.label}
              onAction={item.onAction}
            />
          ))}
        </Grid2>
        <Grid2 size={6} display="flex" justifyContent="flex-end" gap={2}>
          {saveCancelButtonProps.map((item, index) => (
            <FormButton
              key={`save-cancel-button-${index}`}
              disabled={item.disabled}
              label={item.label}
              onAction={item.onAction}
            />
          ))}
        </Grid2>
      </Grid2>
    </Grid2>
  );
};

export default DetailsView;
