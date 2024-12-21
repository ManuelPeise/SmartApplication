import { Box } from "@mui/material";
import React, { PropsWithChildren } from "react";
import { StyledListItem } from "src/_components/_styled/StyledListItem";

interface IProps extends PropsWithChildren {
  imageSrc: string;
  childWidth?: string;
}

const EmailProviderSelection: React.FC<IProps> = (props) => {
  const { imageSrc, childWidth, children } = props;

  return (
    <Box display="flex" padding={3}>
      <StyledListItem
        divider
        sx={{ display: "flex", justifyContent: "space-between" }}
      >
        <Box
          component="img"
          sx={{
            padding: 1,
            height: 50,
            width: 50,
          }}
          alt="email provider logo"
          src={imageSrc}
        />
        <Box width={childWidth ?? "100%"}>{children}</Box>
      </StyledListItem>
    </Box>
  );
};

export default EmailProviderSelection;
