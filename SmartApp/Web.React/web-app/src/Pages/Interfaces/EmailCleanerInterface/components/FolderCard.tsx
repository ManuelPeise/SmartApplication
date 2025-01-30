import { FolderRounded } from "@mui/icons-material";
import { Box, Card, CardContent, Typography } from "@mui/material";
import React from "react";
import { colors } from "src/_lib/colors";

interface IProps {
  itemsCount: number;
  name: string;
  color?: string;
  expanded: boolean;
}

const FolderCard: React.FC<IProps> = (props) => {
  const { itemsCount, name, color, expanded } = props;
  return (
    <Card sx={{ backgroundColor: color, minHeight: 70 }}>
      <CardContent
        sx={{
          display: "flex",
          flexDirection: "row",
          justifyContent: "center",
          alignItems: "flex-start",
          height: "100%",
          padding: 0,
          "&.MuiCardContent-root": {
            padding: 0,
            margin: 0,
          },
        }}
      >
        <Box
          display="flex"
          justifyContent="center"
          alignItems="center"
          height="100%"
        >
          <FolderRounded
            sx={{ width: 40, height: 40, color: colors.lighter }}
          />
        </Box>
        <Box
          height="100%"
          width="75%"
          display={expanded ? "flex" : "none"}
          flexDirection="column"
          p={1}
        >
          <Typography textAlign="end">{itemsCount}</Typography>
          <Typography textAlign="end">{name}</Typography>
        </Box>
      </CardContent>
    </Card>
  );
};

export default FolderCard;
