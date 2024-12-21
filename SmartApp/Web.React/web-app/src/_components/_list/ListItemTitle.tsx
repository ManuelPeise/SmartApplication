import { ListItem, ListItemText, Typography } from "@mui/material";
import React from "react";
import { colors } from "src/_lib/colors";

interface IProps {
  title: string;
  description: string;
}

const ListItemTitle: React.FC<IProps> = (props) => {
  const { title, description } = props;

  return (
    <ListItem
      sx={{
        width: "100%",
        display: "flex",
        flexDirection: "column",
        alignItems: "flex-start",
        padding: "0px 20px",
        marginBottom: "1rem",
      }}
    >
      <ListItemText>
        <Typography
          variant="h4"
          fontStyle="italic"
          color={colors.typography.darkgray}
        >
          {title}
        </Typography>
      </ListItemText>
      <ListItemText>
        {" "}
        <Typography variant="h6" color={colors.typography.darkgray}>
          {description}
        </Typography>
      </ListItemText>
    </ListItem>
  );
};

export default ListItemTitle;
