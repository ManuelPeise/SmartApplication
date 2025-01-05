import { Box } from "@mui/material";
import React from "react";
import SettingsLayout, {
  SettingsListItem,
} from "src/_components/Layouts/SettingsLayout";
import { colors } from "src/_lib/colors";

interface IProps {
  items: SettingsListItem[];
  selectedSection: number;
}

const SettingsPage: React.FC<IProps> = (props) => {
  const { items, selectedSection } = props;

  return (
    <Box
      display="flex"
      justifyContent="center"
      alignItems="center"
      height="100%"
      bgcolor={colors.background}
    >
      <Box
        display="flex"
        justifyContent="center"
        alignItems="center"
        width={{ xs: "100%", sm: "100%", md: "100%", lg: "98%", xl: "98%" }}
        height="95%"
      >
        <SettingsLayout
          items={items}
          selectedSection={selectedSection}
        ></SettingsLayout>
      </Box>
    </Box>
  );
};

export default SettingsPage;
