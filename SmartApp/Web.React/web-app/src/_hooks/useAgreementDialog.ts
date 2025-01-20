import React from "react";

export const useAgreementDialog = (
  initialState: boolean,
  agreementText: string,
  callback: (state: boolean) => void
) => {
  const originalStateRef = React.useRef(initialState);
  const [open, setOpen] = React.useState<boolean>(false);

  if (initialState == null) {
    throw new Error("Initial state could not be null!");
  }

  const onAggree = React.useCallback(
    (state: boolean) => {
      callback(state);
    },
    [callback]
  );

  const onCancel = React.useCallback(() => {
    callback(originalStateRef.current);
  }, [originalStateRef, callback]);

  return {
    open,
    agreementText,
    setOpen,
    onAggree,
    onCancel,
  };
};
