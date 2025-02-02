import React from "react";

const useViewPortRendering = <TModel>(
  elements: TModel[],
  viewPortHeight: number,
  maxElements: number,
  elementsToReload: number,
  depencies: any[]
) => {
  const [renderElements, setRenderElements] = React.useState<TModel[]>(
    elements.slice(0, maxElements)
  );

  const lastRenderedElementRef = React.useRef<HTMLElement>(null);

  const onReload = React.useCallback(() => {
    if (lastRenderedElementRef != null) {
      const minReloadContainerHeight =
        lastRenderedElementRef.current.scrollTop + viewPortHeight - 50;
    }
  }, [viewPortHeight]);

  return {
    lastRenderedElementRef,
  };
};
