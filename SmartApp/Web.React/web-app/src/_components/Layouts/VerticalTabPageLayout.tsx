import { Grid2, Paper, Typography } from "@mui/material";
import React, { PropsWithChildren } from "react";
import { colors } from "src/_lib/colors";
import VerticalTabListMenu, {
  VerticalTabListListItem,
} from "../Lists/VerticalTabListMenu";
import LoadingIndicator from "../Loading/LoadingIndicator";

interface IVertivalTabPageHeaderProps extends PropsWithChildren {
  pageTitle: string;
  isLoading?: boolean;
}

interface IProps extends PropsWithChildren {
  containerId: string;
  pageTitle: string;
  selectedTab: number;
  tabItems: VerticalTabListListItem[];
  headerChildComponent?: JSX.Element;
  maxHeight?: number;
  isLoading?: boolean;
}

const VerticalTabPageHeader: React.FC<IVertivalTabPageHeaderProps> = (
  props
) => {
  const { pageTitle, children, isLoading } = props;

  return (
    <Grid2 id="vertival-tab-header" size={12}>
      <Paper
        sx={{
          width: "100%",
          paddingTop: 2,
          paddingBottom: 0,
        }}
      >
        <Grid2
          display="flex"
          alignItems="baseline"
          justifyContent="space-between"
          paddingBottom={2}
        >
          <Grid2 paddingLeft={4} width="80%" minWidth="30rem">
            <Typography
              variant="h4"
              fontStyle="italic"
              color={colors.typography.blue}
            >
              {pageTitle}
            </Typography>
          </Grid2>
          <Grid2
            display="flex"
            flexDirection="row"
            justifyContent="flex-end"
            alignItems="baseline"
            paddingRight={4}
            width="20%"
            minWidth="15rem"
          >
            {children}
          </Grid2>
        </Grid2>
        <LoadingIndicator isLoading={isLoading ?? false} />
      </Paper>
    </Grid2>
  );
};

const VerticalTabPageLayout: React.FC<IProps> = (props) => {
  const {
    containerId,
    pageTitle,
    selectedTab,
    tabItems,
    headerChildComponent,
    children,
    maxHeight,
    isLoading,
  } = props;

  return (
    <Grid2
      id={containerId}
      width="100%"
      height="inherit"
      display="flex"
      flexDirection="column"
      p={2}
      gap={3}
    >
      <VerticalTabPageHeader pageTitle={pageTitle} isLoading={isLoading}>
        {headerChildComponent}
      </VerticalTabPageHeader>
      <Grid2 id="vertical-tab-body" size={12} height="inherit">
        <Paper
          sx={{
            display: "flex",
            flexDirection: "row",
            height: `${maxHeight}px`,
            minWidth: "900px",
          }}
          elevation={4}
        >
          <Grid2
            height="inherit"
            size={12}
            display="flex"
            flexDirection="row"
            maxWidth="300px"
          >
            <VerticalTabListMenu selectedTab={selectedTab} items={tabItems} />
          </Grid2>
          <Grid2
            id={`${containerId}-body-container`}
            size={10}
            padding={2}
            paddingRight={3}
            width="100%"
            height="inherit"
          >
            {children}
          </Grid2>
        </Paper>
      </Grid2>
    </Grid2>
  );
};

export default VerticalTabPageLayout;
