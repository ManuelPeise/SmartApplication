import { Box, IconButton, ListItem, Tooltip, Typography } from "@mui/material";
import React, { PropsWithChildren } from "react";

interface IProps extends PropsWithChildren {
  label: string;
  toolTipLabel: string;
  icon: JSX.Element;
  marginTop?: string;
  additionalFontSize?: string;
  additionalFontWeight?: string;
  additionalFontStyle?: "italic";
  onClick?: () => void;
}

const ListItemInputWithIcon: React.FC<IProps> = (props) => {
  const {
    label,
    toolTipLabel,
    icon,
    additionalFontSize,
    additionalFontStyle,
    additionalFontWeight,
    marginTop,
    children,
    onClick,
  } = props;
  return (
    <ListItem sx={{ width: "100%" }} divider>
      <Box
        width="100%"
        display="flex"
        flexDirection="column"
        justifyContent="space-between"
        alignItems="baseline"
      >
        <Box
          width="100%"
          display="flex"
          justifyContent="space-between"
          alignItems="baseline"
        >
          <Typography
            sx={{
              fontSize: additionalFontSize,
              marginTop: marginTop,
              fontStyle: additionalFontStyle,
              fontWeight: additionalFontWeight,
            }}
          >
            {label}
          </Typography>
          <Tooltip
            title={toolTipLabel}
            children={
              <IconButton
                size="small"
                onClick={onClick && onClick}
                children={icon}
              />
            }
          />
        </Box>
        <Box
          width="100%"
          display="flex"
          justifyContent="flex-end"
          minWidth="250px"
          bgcolor="green"
        >
          {children}
        </Box>
      </Box>
    </ListItem>
  );
};

export default ListItemInputWithIcon;
