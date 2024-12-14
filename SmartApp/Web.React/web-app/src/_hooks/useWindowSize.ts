import React from "react";

export const useWindowSize = () => {
  const [windowSize, setWindowSize] = React.useState<{
    width: number;
    height: number;
  }>({
    width: 0,
    height: 0,
  });

  React.useEffect(() => {
    const handleRezize = () => {
      setWindowSize({
        width: window.innerWidth,
        height: window.innerHeight,
      });
    };

    window.addEventListener("resize", handleRezize);

    handleRezize();

    return () => window.removeEventListener("resize", handleRezize);
  }, []);

  return windowSize;
};
