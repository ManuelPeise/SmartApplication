import { Box } from "@mui/material";
import React, { CSSProperties, PropsWithChildren } from "react";
import { StyledListItem } from "../_styled/StyledListItem";
import TypoGraphy from "../_labels/TypoGraphy";

interface IProps extends PropsWithChildren {
  description: string;
  childWidth?: string;
  cssTextProperties?: CSSProperties;
}

const ListItemInput: React.FC<IProps> = (props) => {
  const { description, childWidth, children, cssTextProperties } = props;

  return (
    <StyledListItem
      divider
      sx={{
        marginTop: 2,
        display: "flex",
        justifyContent: "space-between",
      }}
    >
      <Box
        component="div"
        sx={{
          padding: 1,
        }}
      >
        <TypoGraphy
          label={description}
          fontSize={{
            xs: ".8rem",
            sm: ".8rem",
            md: "1rem",
            lg: "1.2rem",
            xl: "1.2rem",
          }}
          style={cssTextProperties}
        />
      </Box>
      <Box
        sx={{ display: "flex", justifyContent: "flex-end" }}
        width={childWidth ?? "100%"}
      >
        {children}
      </Box>
    </StyledListItem>
  );
};

export default ListItemInput;
