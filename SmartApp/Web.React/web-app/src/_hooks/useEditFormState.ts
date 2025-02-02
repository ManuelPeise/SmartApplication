import React from "react";

export type FormState = {
  isModified: boolean;
  revertDialogOpen: boolean;
};

export type UseEditFormState<T> = {
  modelState: T;
  formState: FormState;
  handleCloseDialog: (callback: () => void) => void;
  handleModelStateChanged: (partialModel: Partial<T>) => void;
  handleFormStatedChanged: (partialState: Partial<FormState>) => void;
  handleRevertChangesAndCloseDialog: (callback: (id: number) => void) => void;
};

export const useEditFormState = <T>(
  initialState: T,
  tempUserId: number
): UseEditFormState<T> => {
  const originalState = React.useRef(initialState);

  const [formState, setFormState] = React.useState<FormState>({
    isModified: false,
    revertDialogOpen: false,
  });

  const [modelState, setModelState] = React.useState<T>(initialState);

  const handleFormStatedChanged = React.useCallback(
    (partialState: Partial<FormState>) => {
      setFormState({ ...formState, ...partialState });
    },
    [formState]
  );

  const handleModelStateChanged = React.useCallback(
    (partialModel: Partial<T>) => {
      setModelState({ ...modelState, ...partialModel });

      if (formState && !formState?.isModified) {
        handleFormStatedChanged({ isModified: true });
      }
    },
    [modelState, formState, handleFormStatedChanged]
  );

  const handleRevertChangesAndCloseDialog = React.useCallback(
    (callback: (id: number) => void) => {
      setModelState(originalState.current);
      handleFormStatedChanged({ isModified: false, revertDialogOpen: false });

      callback(tempUserId);
    },
    [tempUserId, handleFormStatedChanged]
  );

  const handleCloseDialog = React.useCallback(
    (callback: () => void) => {
      handleFormStatedChanged({ revertDialogOpen: false });
      callback();
    },
    [handleFormStatedChanged]
  );

  return {
    modelState: modelState,
    formState: formState,
    handleCloseDialog: handleCloseDialog,
    handleFormStatedChanged: handleFormStatedChanged,
    handleModelStateChanged: handleModelStateChanged,
    handleRevertChangesAndCloseDialog: handleRevertChangesAndCloseDialog,
  };
};
