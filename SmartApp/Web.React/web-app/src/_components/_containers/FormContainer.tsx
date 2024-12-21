import { Container, Grid2 } from "@mui/material";
import React, { PropsWithChildren } from "react";
import FormButton from "../_buttons/FormButton";
import { StyledBox } from "../_styled/StyledBox";

export type AdditionalButtonProps = {
  label: string;
  disabled: boolean;
  action: () => Promise<void> | void;
};

interface IProps extends PropsWithChildren {
  evaluation: 1 | 2 | 4;
  fullWidth?: boolean;
  buttonProps: AdditionalButtonProps[];
  additionalButtonProps?: AdditionalButtonProps[];
}

const FormContainer: React.FC<IProps> = (props) => {
  const { children, buttonProps, additionalButtonProps } = props;
  return (
    <StyledBox
      sx={{
        flexDirection: "column",
        justifyContent: "space-around",
      }}
    >
      <Container>{children}</Container>
      <Container sx={{ display: "flex", justifyContent: "space-between" }}>
        <Grid2 display="flex" alignItems="baseline" gap={2}>
          {additionalButtonProps?.map((props, key) => {
            return (
              <FormButton
                key={key}
                label={props.label}
                disabled={props.disabled}
                onAction={props.action}
              />
            );
          })}
        </Grid2>
        <Grid2 display="flex" alignItems="baseline" gap={2}>
          {buttonProps.map((props, key) => {
            return (
              <FormButton
                key={key}
                label={props.label}
                disabled={props.disabled}
                onAction={props.action}
              />
            );
          })}
        </Grid2>
      </Container>
    </StyledBox>
  );
};

export default FormContainer;
