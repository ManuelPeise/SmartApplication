import React from "react";

export type ColumnDefinition<TModel> = {
  name: keyof TModel;
  percentageWidth: number;
  width?: number;
};

export const usePercentageColumnWidth = <T>(cols: ColumnDefinition<T>[]) => {
  const ref = React.useRef<HTMLDivElement>(null);

  const [columns, setColumns] = React.useState<ColumnDefinition<T>[]>(cols);

  React.useEffect(() => {
    const updateWidth = () => {
      if (ref.current) {
        const columnCopy = [...columns];
        const parentWidth = ref.current.parentElement?.clientWidth;

        console.log("Parent", parentWidth);
        columnCopy.forEach((col) => {
          col.width = (parentWidth / 100) * col.percentageWidth;
        });

        setColumns(columnCopy);
      }
    };

    updateWidth();
    window.addEventListener("resize", updateWidth);
    return () => window.removeEventListener("resize", updateWidth);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return { containerRef: ref, columns };
};
