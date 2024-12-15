import React, { PropsWithChildren } from "react";
import { useWindowSize } from "src/_hooks/useWindowSize";
import { colors } from "src/_lib/colors";

const RootLayout: React.FC<PropsWithChildren> = (props) => {
  const { children } = props;
  const { width, height } = useWindowSize();

  return (
    <div
      id="root-layout"
      style={{
        width: `${width}px`,
        height: `${height}px`,
        display: "flex",
        backgroundColor: colors.background.lightgray,
        overflowX: "scroll",
      }}
    >
      {children}
    </div>
  );
};

export default RootLayout;
