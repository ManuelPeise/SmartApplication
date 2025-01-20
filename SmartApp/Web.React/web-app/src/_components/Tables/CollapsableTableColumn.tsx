import {
  NavigateBeforeRounded,
  NavigateNextRounded,
} from "@mui/icons-material";
import { Grid2, IconButton, Typography } from "@mui/material";
import React, { PropsWithChildren } from "react";
import { useI18n } from "src/_hooks/useI18n";

export type TableCellProps<TModel> = {
  cellKey: string;
  padding: number;
  maxHeight: number;
  hasToolTip?: boolean;
  cellProps: TModel;
};

// type TableColumnPropsProps<TModel>
//   PropsWithChildren & {
//     index: number;
//     maxHeight: number;
//   };

// function CollapsibleTableColumn<TModel>(props: TableColumnPropsProps<TModel>) {
//   const {
//     id,
//     isCollapsible,
//     isExpanded,
//     collapsedWidth,
//     minWidth,
//     headerResourceKey,
//     maxHeight,
//     hasRightBorder,
//     hasLeftBorder,
//     children,
//   } = props;

//   const { getResource } = useI18n();
//   const [isCollapsed, setIscollapsed] = React.useState<boolean>(isExpanded);

//   return (
//     <Grid2
//       id="column-container"
//       sx={
//         isCollapsed
//           ? {
//               display: "block",
//               position: "relative",
//               margin: "0px 0",
//               width: isCollapsed ? collapsedWidth : minWidth,
//               height: maxHeight,
//               borderRight: hasRightBorder ? "1px solid lightgray" : "",
//               borderLeft: hasLeftBorder ? "1px solid lightgray" : "",
//               borderBottom: "1px solid lightgray",
//             }
//           : {
//               display: "block",
//               position: "relative",
//               margin: "0px 0",
//               width: isCollapsed ? collapsedWidth : minWidth,
//               height: maxHeight,
//               borderTop: "1px solid lightgray",
//               borderBottom: "1px solid lightgray",
//               borderRight: hasRightBorder ? "1px solid lightgray" : "",
//               borderLeft: hasLeftBorder ? "1px solid lightgray" : "",
//               overflow: "hidden",
//             }
//       }
//     >
//       {/* // t-head */}
//       <Grid2
//         id={`colomn-header-${id as string}`}
//         minWidth={isCollapsed ? maxHeight : ""}
//         onClick={setIscollapsed.bind(null, !isCollapsed)}
//         sx={
//           isCollapsed
//             ? {
//                 position: "relative",
//                 background: "none",
//                 transformOrigin: "25px 25px",
//                 transform: "rotate(90deg)",
//                 width: minWidth,
//                 display: "flex",
//                 flexDirection: "row-reverse",
//                 justifyContent: "flex-end",
//                 alignItems: "center",
//                 borderLeft: "1px solid lightgray",
//               }
//             : {
//                 // position: "fixed",
//                 // background: "#fff",
//                 // width: minWidth,
//                 // zIndex: 999,
//                 textTransform: "uppercase",
//                 padding: "0px 10px",
//                 margin: "0 0 0 0",
//                 lineHeight: "24px",
//                 display: "flex",
//                 justifyContent: "space-between",
//                 flexDirection: "row",
//                 alignItems: "center",
//                 borderBottom: "1px solid lightgray",
//                 borderRight: "1px solid lightgray",
//               }
//         }
//       >
//         <Typography
//           sx={
//             isCollapsed
//               ? {
//                   fontWeight: "600",
//                   paddingLeft: 2,
//                 }
//               : {
//                   fontWeight: "600",
//                 }
//           }
//         >
//           {getResource(headerResourceKey)}
//         </Typography>
//         <IconButton
//           size="small"
//           sx={
//             isCollapsed
//               ? {
//                   display: "flex",
//                   justifyContent: "center",
//                   justifyItems: "center",
//                   alignItems: "center",
//                   width: collapsedWidth,
//                   height: collapsedWidth,
//                 }
//               : {
//                   display: "flex",
//                   justifyContent: "center",
//                   alignItems: "center",
//                   width: collapsedWidth,
//                   height: collapsedWidth,
//                 }
//           }
//           onClick={
//             isCollapsible ? setIscollapsed.bind(null, !isCollapsed) : null
//           }
//         >
//           {isCollapsed ? (
//             <NavigateNextRounded
//               sx={{
//                 rotate: isCollapsed ? "-90deg" : "none",
//               }}
//             />
//           ) : (
//             <NavigateBeforeRounded sx={{}} />
//           )}
//         </IconButton>
//       </Grid2>
//       {/* // t-body */}
//       {!isCollapsed && (
//         <Grid2 id={`column-body-${id as string}`}>{children}</Grid2>
//       )}
//     </Grid2>
//   );
// }

// export default CollapsibleTableColumn;
